using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// ペア処理待ちのファイルグループ
    /// </summary>
    internal class PendingFileGroup
    {
        public string BaseName { get; set; } = string.Empty;
        public string? JpegFile { get; set; }
        public string? RawFile { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string SequenceNumber { get; set; } = string.Empty;
        
        public bool IsComplete => !string.IsNullOrEmpty(JpegFile) && !string.IsNullOrEmpty(RawFile);
        public bool HasJpeg => !string.IsNullOrEmpty(JpegFile);
        public bool HasRaw => !string.IsNullOrEmpty(RawFile);
    }
    /// <summary>
    /// ファイル監視サービスクラス
    /// </summary>
    public class FileMonitorService
    {
        private readonly LogManager _logManager;
        private readonly FileOperationService _fileOperationService;
        private PreviewService? _previewService;
        private FileSystemWatcher? _fileSystemWatcher;
        private FileSystemWatcher? _destinationWatcher;
        private Timer? _pollingTimer;
        private CancellationTokenSource? _cancellationTokenSource;
        
        private AppSettings? _settings;
        private bool _isMonitoring;

        private readonly HashSet<string> _processedFiles = new HashSet<string>();
        private readonly object _processedFilesLock = new object();
        
        // JPEG+RAWファイルのペア処理用
        private readonly Dictionary<string, PendingFileGroup> _pendingFileGroups = new Dictionary<string, PendingFileGroup>();
        private readonly object _pendingFileGroupsLock = new object();
        
        // 移動先フォルダの重複処理防止用
        private readonly HashSet<string> _processingDestinationFiles = new HashSet<string>();
        private readonly object _processingDestinationFilesLock = new object();

        /// <summary>
        /// ファイル処理完了イベント
        /// </summary>
        public event EventHandler<FileProcessedEventArgs>? FileProcessed;

        /// <summary>
        /// エラー発生イベント
        /// </summary>
        public event EventHandler<ErrorEventArgs>? ErrorOccurred;

        public FileMonitorService(LogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            _fileOperationService = new FileOperationService(_logManager);
            
            _fileOperationService.FileProcessed += OnFileProcessed;
            _fileOperationService.ErrorOccurred += OnErrorOccurred;
        }

        /// <summary>
        /// 監視開始
        /// </summary>
        public async Task StartMonitoringAsync(AppSettings settings)
        {
            if (_isMonitoring)
            {
                throw new InvalidOperationException("既に監視が開始されています");
            }

            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            if (!Directory.Exists(_settings.SourceFolder))
            {
                throw new DirectoryNotFoundException($"監視フォルダが存在しません: {_settings.SourceFolder}");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _isMonitoring = true;

            ClearProcessedFiles();

            switch (_settings.MonitorMode)
            {
                case MonitorMode.Immediate:
                    await StartImmediateMonitoringAsync();
                    break;
                case MonitorMode.Polling:
                    await StartPollingMonitoringAsync();
                    break;
                default:
                    throw new ArgumentException($"未サポートの監視モード: {_settings.MonitorMode}");
            }


            // プレビューサービスの初期化
            _logManager.LogInfo($"外部ソフトウェア設定確認: AutoActivate={_settings.AutoActivateExternalSoftware}, Path='{_settings.ExternalSoftwarePath}'");
            
            if (_settings.AutoActivateExternalSoftware && !string.IsNullOrEmpty(_settings.ExternalSoftwarePath))
            {
                try
                {
                    _logManager.LogInfo($"プレビューサービス初期化を試行: {_settings.ExternalSoftwarePath}");
                    _previewService = new PreviewService(_settings.ExternalSoftwarePath);
                    _logManager.LogInfo($"プレビューサービスを初期化しました: {_settings.ExternalSoftwarePath}");
                    
                    // 移動先フォルダの監視を開始（プレビュー表示用）
                    _logManager.LogInfo("移動先フォルダの監視を開始します");
                    await StartDestinationFolderMonitoringAsync();
                }
                catch (Exception ex)
                {
                    _logManager.LogError($"プレビューサービスの初期化に失敗しました: {ex.Message}");
                    _previewService = null;
                }
            }
            else
            {
                if (!_settings.AutoActivateExternalSoftware)
                {
                    _logManager.LogInfo("外部ソフトウェア自動アクティブ化が無効になっています");
                }
                else if (string.IsNullOrEmpty(_settings.ExternalSoftwarePath))
                {
                    _logManager.LogInfo("外部ソフトウェアのパスが設定されていません");
                }
                else
                {
                    _logManager.LogWarning("外部ソフトウェア連携が無効です（不明な理由）");
                }
            }

            _logManager.LogInfo($"ファイル監視を開始しました - モード: {_settings.MonitorMode}, フォルダ: {_settings.SourceFolder}");
        }

        /// <summary>
        /// 監視停止
        /// </summary>
        public async Task StopMonitoringAsync()
        {
            if (!_isMonitoring)
            {
                return;
            }

            _isMonitoring = false;

            _cancellationTokenSource?.Cancel();

            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = null;

            _destinationWatcher?.Dispose();
            _destinationWatcher = null;

            _pollingTimer?.Dispose();
            _pollingTimer = null;

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            await Task.Delay(100);

            // 監視停止時のファイル削除処理
            await PerformStopTimeCleanupAsync();

            _logManager.LogInfo("ファイル監視を停止しました");
        }

        /// <summary>
        /// 即座監視開始（クロスプラットフォーム対応）
        /// </summary>
        private async Task StartImmediateMonitoringAsync()
        {
            await Task.Run(() =>
            {
                _fileSystemWatcher = new FileSystemWatcher(_settings!.SourceFolder!)
                {
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true,
                    // クロスプラットフォーム対応のための設定
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName,
                    // バッファサイズを増やして取りこぼしを防ぐ
                    InternalBufferSize = 8192 * 4
                };

                _fileSystemWatcher.Created += async (sender, e) => await OnFileCreatedAsync(e.FullPath);
                _fileSystemWatcher.Renamed += async (sender, e) => await OnFileCreatedAsync(e.FullPath);
                _fileSystemWatcher.Changed += async (sender, e) => await OnFileCreatedAsync(e.FullPath);
                
                // エラーハンドリングを追加
                _fileSystemWatcher.Error += (sender, e) =>
                {
                    _logManager.LogError($"FileSystemWatcher エラー: {e.GetException().Message}");
                    // エラー時にポーリング監視にフォールバック
                    _ = Task.Run(async () => await StartPollingFallbackAsync());
                };

                var platform = Environment.OSVersion.Platform;
                _logManager.LogInfo($"FileSystemWatcherによる即座監視を開始しました (プラットフォーム: {platform})");
            });

            // 既存ファイルの処理
            await ProcessExistingFilesAsync();
        }

        /// <summary>
        /// FileSystemWatcherエラー時のポーリングフォールバック
        /// </summary>
        private async Task StartPollingFallbackAsync()
        {
            _logManager.LogWarning("FileSystemWatcherが失敗したため、ポーリングモードにフォールバックします");
            
            // FileSystemWatcherを停止
            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = null;
            
            // ポーリング監視を開始
            await Task.Delay(1000); // 1秒待機
            _pollingTimer = new Timer(async _ => await PollingCheckAsync(), 
                null, TimeSpan.Zero, TimeSpan.FromSeconds(Math.Max(_settings!.PollingInterval, 5)));
        }

        /// <summary>
        /// ポーリング監視開始
        /// </summary>
        private async Task StartPollingMonitoringAsync()
        {
            await Task.Run(() =>
            {
                _pollingTimer = new Timer(async _ => await PollingCheckAsync(), 
                    null, TimeSpan.Zero, TimeSpan.FromSeconds(_settings!.PollingInterval));

                _logManager.LogInfo($"ポーリング監視を開始しました - 間隔: {_settings.PollingInterval}秒");
            });
        }

        /// <summary>
        /// 監視開始時の既存ファイル処理
        /// </summary>
        private async Task ProcessExistingFilesAsync()
        {
            try
            {
                if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                {
                    return;
                }

                _logManager.LogInfo("監視開始時の既存ファイルを処理します");

                var patterns = FileExtensions.GetSearchPatterns(_settings!.MonitorJPEG, _settings.MonitorRAW);
                var filesToProcess = new List<string>();

                foreach (var pattern in patterns)
                {
                    var files = Directory.GetFiles(_settings.SourceFolder!, pattern, SearchOption.TopDirectoryOnly);
                    filesToProcess.AddRange(files.Where(file => !IsFileAlreadyProcessed(file)));
                }

                if (filesToProcess.Count > 0)
                {
                    _logManager.LogInfo($"既存ファイル {filesToProcess.Count} 個を処理対象として検出しました");

                    foreach (var file in filesToProcess)
                    {
                        if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                        {
                            break;
                        }

                        await OnFileCreatedAsync(file);
                    }

                    _logManager.LogInfo("既存ファイル処理が完了しました");
                }
                else
                {
                    _logManager.LogInfo("処理対象の既存ファイルはありませんでした");
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"既存ファイル処理中にエラーが発生しました: {ex.Message}");
                OnErrorOccurred("既存ファイル処理エラー", ex);
            }
        }

        /// <summary>
        /// ファイル作成イベントハンドラ
        /// </summary>
        private async Task OnFileCreatedAsync(string filePath)
        {
            try
            {
                if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                {
                    return;
                }

                if (!ShouldProcessFile(filePath))
                {
                    return;
                }

                if (IsFileAlreadyProcessed(filePath))
                {
                    return;
                }

                // ファイルを処理中としてマークして重複処理を防ぐ
                MarkFileAsProcessed(filePath);

                await Task.Delay(1000, _cancellationTokenSource?.Token ?? CancellationToken.None);

                if (!await _fileOperationService.WaitForFileAvailableAsync(filePath))
                {
                    OnErrorOccurred($"ファイルが使用中でアクセスできません: {filePath}");
                    // ファイルアクセス失敗時は処理済みリストから削除して再試行可能にする
                    RemoveFileFromProcessed(filePath);
                    return;
                }

                // JPEG+RAW両方を監視している場合はペア処理を試行
                if (_settings!.MonitorJPEG && _settings.MonitorRAW)
                {
                    await ProcessFileWithPairingAsync(filePath);
                }
                else
                {
                    // 単体ファイル処理
                    await ProcessSingleFileAsync(filePath);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"ファイル処理中にエラーが発生しました: {filePath}", ex);
                // エラー時は処理済みリストから削除して再試行可能にする
                RemoveFileFromProcessed(filePath);
            }
        }

        /// <summary>
        /// ポーリングチェック
        /// </summary>
        private async Task PollingCheckAsync()
        {
            try
            {
                if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                {
                    return;
                }

                var patterns = FileExtensions.GetSearchPatterns(_settings!.MonitorJPEG, _settings.MonitorRAW);
                var filesToProcess = new List<string>();

                foreach (var pattern in patterns)
                {
                    var files = Directory.GetFiles(_settings.SourceFolder!, pattern, SearchOption.TopDirectoryOnly);
                    filesToProcess.AddRange(files.Where(file => !IsFileAlreadyProcessed(file)));
                }

                foreach (var file in filesToProcess)
                {
                    if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                    {
                        break;
                    }

                    await OnFileCreatedAsync(file);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred("ポーリングチェック中にエラーが発生しました", ex);
            }
        }

        /// <summary>
        /// ファイルが処理対象かどうかを判定
        /// </summary>
        private bool ShouldProcessFile(string filePath)
        {
            return FileExtensions.ShouldProcessFile(filePath, _settings!.MonitorJPEG, _settings.MonitorRAW);
        }

        /// <summary>
        /// ファイルが既に処理済みかどうかを確認
        /// </summary>
        private bool IsFileAlreadyProcessed(string filePath)
        {
            lock (_processedFilesLock)
            {
                return _processedFiles.Contains(filePath);
            }
        }

        /// <summary>
        /// ファイルを処理済みとしてマーク
        /// </summary>
        private void MarkFileAsProcessed(string filePath)
        {
            lock (_processedFilesLock)
            {
                _processedFiles.Add(filePath);
            }
        }

        /// <summary>
        /// 処理済みファイルリストをクリア
        /// </summary>
        private void ClearProcessedFiles()
        {
            lock (_processedFilesLock)
            {
                _processedFiles.Clear();
            }
        }

        /// <summary>
        /// 処理済みファイルリストからファイルを削除
        /// </summary>
        private void RemoveFileFromProcessed(string filePath)
        {
            lock (_processedFilesLock)
            {
                _processedFiles.Remove(filePath);
            }
        }

        /// <summary>
        /// ファイル処理完了イベントハンドラ
        /// </summary>
        private void OnFileProcessed(object? sender, FileProcessedEventArgs e)
        {
            FileProcessed?.Invoke(this, e);
        }

        /// <summary>
        /// エラー発生イベントハンドラ
        /// </summary>
        private void OnErrorOccurred(object? sender, ErrorEventArgs e)
        {
            ErrorOccurred?.Invoke(this, e);
        }

        /// <summary>
        /// エラー発生イベントハンドラ（内部用）
        /// </summary>
        private void OnErrorOccurred(string message, Exception? exception = null)
        {
            ErrorOccurred?.Invoke(this, new ErrorEventArgs(message, exception));
        }

        /// <summary>
        /// 新しいファイル名を生成
        /// </summary>
        private string GenerateNewFileName(string originalFilePath)
        {
            var sequenceNumber = _settings!.GetNextSequenceNumber();
            return GenerateNewFileNameWithSequence(originalFilePath, sequenceNumber);
        }

        /// <summary>
        /// 指定された連番で新しいファイル名を生成
        /// </summary>
        private string GenerateNewFileNameWithSequence(string originalFilePath, string sequenceNumber)
        {
            var extension = Path.GetExtension(originalFilePath);
            var fileInfo = new FileInfo(originalFilePath);
            
            string prefix;
            if (_settings!.PrefixType == PrefixType.DateTime)
            {
                // 撮影日時またはファイル作成日時を使用
                var dateTime = GetPhotoDateTime(originalFilePath) ?? fileInfo.CreationTime;
                prefix = _settings.GetDateTimePrefix(dateTime);
            }
            else
            {
                prefix = _settings.FilePrefix;
            }

            var separator = _settings.GetSeparator();
            return $"{prefix}{separator}{sequenceNumber}{extension}";
        }

        /// <summary>
        /// 写真の撮影日時を取得（EXIF情報から）
        /// </summary>
        private DateTime? GetPhotoDateTime(string filePath)
        {
            try
            {
                // 簡単なEXIF読み取り実装
                // 本格的な実装では専用ライブラリを使用
                return null; // 今回は簡易実装でファイル作成日時を使用
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 移動先フォルダの監視を開始（リアルタイムプレビュー用）
        /// </summary>
        private async Task StartDestinationFolderMonitoringAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    _destinationWatcher = new FileSystemWatcher(_settings!.DestinationFolder!)
                    {
                        IncludeSubdirectories = false,
                        EnableRaisingEvents = true,
                        NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName
                    };

                    _destinationWatcher.Created += async (sender, e) => await OnDestinationFileCreatedAsync(e.FullPath);
                    _destinationWatcher.Changed += async (sender, e) => await OnDestinationFileCreatedAsync(e.FullPath);

                    _logManager.LogInfo("移動先フォルダのリアルタイム監視を開始しました");
                });
            }
            catch (Exception ex)
            {
                OnErrorOccurred("移動先フォルダ監視の開始に失敗しました", ex);
            }
        }

        /// <summary>
        /// 移動先フォルダにファイルが作成された時の処理
        /// </summary>
        private async Task OnDestinationFileCreatedAsync(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                _logManager.LogInfo($"移動先フォルダでファイル変更イベント検出: {fileName}");
                
                if (_cancellationTokenSource?.Token.IsCancellationRequested == true)
                {
                    _logManager.LogInfo("キャンセル要求のため処理を中断");
                    return;
                }

                // 重複処理チェック
                lock (_processingDestinationFilesLock)
                {
                    if (_processingDestinationFiles.Contains(filePath))
                    {
                        _logManager.LogInfo($"既に処理中のファイルのためスキップ: {fileName}");
                        return;
                    }
                    _processingDestinationFiles.Add(filePath);
                }

                try
                {
                    // 画像ファイルかチェック
                    if (!IsImageFile(filePath))
                    {
                        _logManager.LogInfo($"画像ファイルではないためスキップ: {fileName}");
                        return;
                    }

                    _logManager.LogInfo($"画像ファイルを確認、処理を開始: {fileName}");

                    // ファイルが完全に書き込まれるまで少し待つ
                    await Task.Delay(500, _cancellationTokenSource?.Token ?? CancellationToken.None);

                    // ファイルがアクセス可能になるまで待つ
                    if (!await _fileOperationService.WaitForFileAvailableAsync(filePath))
                    {
                        _logManager.LogWarning($"ファイルアクセス不可のため処理をスキップ: {fileName}");
                        return;
                    }

                    await HandleNewImageInDestination(filePath);
                }
                finally
                {
                    // 処理完了後にファイルを処理中リストから削除
                    lock (_processingDestinationFilesLock)
                    {
                        _processingDestinationFiles.Remove(filePath);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logManager.LogInfo("移動先ファイル処理がキャンセルされました");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"移動先ファイル処理エラー: {filePath}, {ex.Message}");
                OnErrorOccurred($"移動先ファイル処理エラー: {filePath}", ex);
            }
        }

        /// <summary>
        /// 移動先フォルダの新しい画像を処理
        /// </summary>
        private Task HandleNewImageInDestination(string imagePath)
        {
            try
            {
                _logManager.LogInfo($"移動先フォルダで新しい画像を検出: {Path.GetFileName(imagePath)}");
                
                if (_previewService != null)
                {
                    _logManager.LogInfo($"プレビューサービスが有効、処理を開始");
                    
                    // UIスレッドをブロックしないよう、Task.Runで実行
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            _logManager.LogInfo($"プレビュー表示処理を開始: {Path.GetFileName(imagePath)}");
                            
                            // プレビューアプリで画像を開く
                            var success = _previewService.OpenFile(imagePath);
                            
                            if (success)
                            {
                                _logManager.LogInfo($"プレビュー表示完了: {Path.GetFileName(imagePath)}");
                            }
                            else
                            {
                                _logManager.LogWarning($"プレビュー表示に失敗しました: {Path.GetFileName(imagePath)}");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logManager.LogError($"プレビュー表示エラー: {ex.Message}");
                        }
                    });
                }
                else
                {
                    _logManager.LogInfo($"プレビューサービスが無効のため、外部ソフトウェア連携をスキップ: {Path.GetFileName(imagePath)}");
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"画像処理エラー: {imagePath}, {ex.Message}");
                OnErrorOccurred($"画像処理エラー: {imagePath}", ex);
            }
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// ファイルが画像ファイルかチェック
        /// </summary>
        private bool IsImageFile(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var imageExtensions = new[] { ".jpg", ".jpeg", ".cr2", ".cr3", ".arw", ".nef", ".rw2", ".orf", ".dng" };
            return imageExtensions.Contains(extension);
        }

        /// <summary>
        /// 最新の画像ファイルを取得
        /// </summary>
        private string? GetLatestImageFile(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    return null;

                var extensions = new[] { ".jpg", ".jpeg", ".cr2", ".cr3", ".arw", ".nef", ".rw2", ".orf", ".dng" };
                var files = Directory.GetFiles(folderPath)
                    .Where(f => extensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .FirstOrDefault();

                return files;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 監視停止時のクリーンアップ処理
        /// </summary>
        private async Task PerformStopTimeCleanupAsync()
        {
            try
            {
                if (_settings!.AutoDeleteRawFiles && _settings.MonitorJPEG && !_settings.MonitorRAW)
                {
                    _logManager.LogInfo("JPEGのみ監視のため、RAWファイル削除処理を実行します");
                    await DeleteUnmonitoredFilesAsync("RAW");
                }
                else if (_settings.AutoDeleteJpegFiles && !_settings.MonitorJPEG && _settings.MonitorRAW)
                {
                    _logManager.LogInfo("RAWのみ監視のため、JPEGファイル削除処理を実行します");
                    await DeleteUnmonitoredFilesAsync("JPEG");
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"停止時クリーンアップエラー: {ex.Message}");
                OnErrorOccurred("停止時クリーンアップエラー", ex);
            }
        }

        /// <summary>
        /// 監視対象外ファイルを削除
        /// </summary>
        private async Task DeleteUnmonitoredFilesAsync(string fileType)
        {
            try
            {
                if (!Directory.Exists(_settings!.SourceFolder))
                {
                    return;
                }

                var deletedCount = 0;
                var extensions = fileType == "RAW" ? FileExtensions.GetRawExtensions() : new[] { "jpg", "jpeg" };

                await Task.Run(() =>
                {
                    foreach (var extension in extensions)
                    {
                        var pattern = $"*.{extension}";
                        var files = Directory.GetFiles(_settings.SourceFolder, pattern, SearchOption.TopDirectoryOnly);

                        foreach (var file in files)
                        {
                            try
                            {
                                File.Delete(file);
                                deletedCount++;
                                _logManager.LogInfo($"{fileType}ファイルを削除しました: {Path.GetFileName(file)}");
                            }
                            catch (Exception ex)
                            {
                                _logManager.LogError($"{fileType}ファイル削除エラー: {Path.GetFileName(file)}, {ex.Message}");
                            }
                        }
                    }
                });

                _logManager.LogInfo($"{fileType}ファイル削除処理完了: {deletedCount}個のファイルを削除しました");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"{fileType}ファイル削除処理エラー: {ex.Message}");
                OnErrorOccurred($"{fileType}ファイル削除処理エラー", ex);
            }
        }

        /// <summary>
        /// ペア処理でファイルを処理
        /// </summary>
        private Task ProcessFileWithPairingAsync(string filePath)
        {
            var baseName = GetFileBaseName(filePath);
            var isJpeg = FileExtensions.IsJPEGFile(filePath);
            var isRaw = FileExtensions.IsRAWFile(filePath);

            lock (_pendingFileGroupsLock)
            {
                if (!_pendingFileGroups.TryGetValue(baseName, out var group))
                {
                    // 新しいグループを作成
                    group = new PendingFileGroup
                    {
                        BaseName = baseName,
                        SequenceNumber = _settings!.GetNextSequenceNumber()
                    };
                    _pendingFileGroups[baseName] = group;
                }

                // ファイルをグループに追加
                if (isJpeg)
                {
                    group.JpegFile = filePath;
                }
                else if (isRaw)
                {
                    group.RawFile = filePath;
                }

                // ペアが完成した場合は処理実行
                if (group.IsComplete)
                {
                    _pendingFileGroups.Remove(baseName);
                    _ = Task.Run(() => ProcessFileGroupAsync(group));
                }
                else
                {
                    // 単体ファイルの場合は即座に処理（タイムアウト処理はCleanupTimeoutGroupsで実行）
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(5000); // 5秒待機
                        lock (_pendingFileGroupsLock)
                        {
                            if (_pendingFileGroups.TryGetValue(baseName, out var timeoutGroup))
                            {
                                _pendingFileGroups.Remove(baseName);
                                _ = Task.Run(() => ProcessFileGroupAsync(timeoutGroup));
                            }
                        }
                    });
                }
            }
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// ファイルグループを処理
        /// </summary>
        private async Task ProcessFileGroupAsync(PendingFileGroup group)
        {
            try
            {
                if (group.HasJpeg && _settings!.MonitorJPEG)
                {
                    await ProcessSingleFileWithSequenceAsync(group.JpegFile!, group.SequenceNumber);
                }

                if (group.HasRaw && _settings!.MonitorRAW)
                {
                    await ProcessSingleFileWithSequenceAsync(group.RawFile!, group.SequenceNumber);
                }

                _logManager.LogInfo($"ファイルグループ処理完了: {group.BaseName} (連番: {group.SequenceNumber})");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"ファイルグループ処理エラー: {group.BaseName}, {ex.Message}");
                OnErrorOccurred($"ファイルグループ処理エラー: {group.BaseName}", ex);
                
                // エラー時は処理済みリストから削除して再試行可能にする
                if (group.HasJpeg)
                {
                    RemoveFileFromProcessed(group.JpegFile!);
                }
                if (group.HasRaw)
                {
                    RemoveFileFromProcessed(group.RawFile!);
                }
            }
        }

        /// <summary>
        /// 単体ファイルを処理
        /// </summary>
        private async Task ProcessSingleFileAsync(string filePath)
        {
            var sequenceNumber = _settings!.GetNextSequenceNumber();
            await ProcessSingleFileWithSequenceAsync(filePath, sequenceNumber);
        }

        /// <summary>
        /// 指定された連番でファイルを処理
        /// </summary>
        private async Task ProcessSingleFileWithSequenceAsync(string filePath, string sequenceNumber)
        {
            var newFileName = GenerateNewFileNameWithSequence(filePath, sequenceNumber);
            _logManager.LogInfo($"リネーム処理: {Path.GetFileName(filePath)} → {newFileName}");
            var success = await _fileOperationService.ProcessFileAsync(filePath, _settings!.DestinationFolder!, newFileName);
            
            if (success)
            {
                // ファイルが正常に移動・削除された場合、処理済みリストから削除
                // 新しい同名ファイルが後で検出された際に再処理できるようにする
                RemoveFileFromProcessed(filePath);
            }
            else
            {
                // 処理に失敗した場合も処理済みリストから削除して再試行可能にする
                RemoveFileFromProcessed(filePath);
            }
        }

        /// <summary>
        /// ファイルのベース名を取得（拡張子を除いた部分）
        /// </summary>
        private string GetFileBaseName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// ペア処理のタイムアウトファイルをクリーンアップ
        /// </summary>
        private void CleanupTimeoutGroups()
        {
            lock (_pendingFileGroupsLock)
            {
                var timeout = TimeSpan.FromMinutes(2); // 2分でタイムアウト
                var expiredKeys = _pendingFileGroups
                    .Where(kvp => DateTime.Now - kvp.Value.CreatedAt > timeout)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    var group = _pendingFileGroups[key];
                    _logManager.LogWarning($"ペア処理タイムアウト: {group.BaseName}");
                    
                    // タイムアウトしたファイルを個別処理
                    if (group.HasJpeg && _settings!.MonitorJPEG)
                    {
                        _ = Task.Run(() => ProcessSingleFileAsync(group.JpegFile!));
                    }
                    if (group.HasRaw && _settings!.MonitorRAW)
                    {
                        _ = Task.Run(() => ProcessSingleFileAsync(group.RawFile!));
                    }
                    
                    _pendingFileGroups.Remove(key);
                }
            }
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            StopMonitoringAsync().Wait();
        }
    }
}