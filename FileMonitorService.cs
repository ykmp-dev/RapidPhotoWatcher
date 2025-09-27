using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace JPEGFolderMonitor
{
    /// <summary>
    /// ファイル監視サービスクラス
    /// </summary>
    public class FileMonitorService
    {
        private readonly LogManager _logManager;
        private readonly FileOperationService _fileOperationService;
        private FileSystemWatcher? _fileSystemWatcher;
        private Timer? _pollingTimer;
        private CancellationTokenSource? _cancellationTokenSource;
        
        private string? _sourceFolder;
        private string? _destinationFolder;
        private string? _filePrefix;
        private MonitorMode _monitorMode;
        private int _pollingInterval;
        private bool _monitorJPEG;
        private bool _monitorRAW;
        private bool _isMonitoring;

        private readonly HashSet<string> _processedFiles = new HashSet<string>();
        private readonly object _processedFilesLock = new object();

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
        public async Task StartMonitoringAsync(
            string sourceFolder,
            string destinationFolder,
            string filePrefix,
            MonitorMode monitorMode,
            int pollingInterval,
            bool monitorJPEG,
            bool monitorRAW)
        {
            if (_isMonitoring)
            {
                throw new InvalidOperationException("既に監視が開始されています");
            }

            _sourceFolder = sourceFolder ?? throw new ArgumentNullException(nameof(sourceFolder));
            _destinationFolder = destinationFolder ?? throw new ArgumentNullException(nameof(destinationFolder));
            _filePrefix = filePrefix ?? throw new ArgumentNullException(nameof(filePrefix));
            _monitorMode = monitorMode;
            _pollingInterval = pollingInterval;
            _monitorJPEG = monitorJPEG;
            _monitorRAW = monitorRAW;

            if (!Directory.Exists(_sourceFolder))
            {
                throw new DirectoryNotFoundException($"監視フォルダが存在しません: {_sourceFolder}");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _isMonitoring = true;

            ClearProcessedFiles();

            switch (_monitorMode)
            {
                case MonitorMode.Immediate:
                    await StartImmediateMonitoringAsync();
                    break;
                case MonitorMode.Polling:
                    await StartPollingMonitoringAsync();
                    break;
                default:
                    throw new ArgumentException($"未サポートの監視モード: {_monitorMode}");
            }

            _logManager.LogInfo($"ファイル監視を開始しました - モード: {_monitorMode}, フォルダ: {_sourceFolder}");
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

            _pollingTimer?.Dispose();
            _pollingTimer = null;

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            await Task.Delay(100);

            _logManager.LogInfo("ファイル監視を停止しました");
        }

        /// <summary>
        /// 即座監視開始
        /// </summary>
        private async Task StartImmediateMonitoringAsync()
        {
            await Task.Run(() =>
            {
                _fileSystemWatcher = new FileSystemWatcher(_sourceFolder!)
                {
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true
                };

                _fileSystemWatcher.Created += async (sender, e) => await OnFileCreatedAsync(e.FullPath);
                _fileSystemWatcher.Renamed += async (sender, e) => await OnFileCreatedAsync(e.FullPath);

                _logManager.LogInfo("FileSystemWatcherによる即座監視を開始しました");
            });
        }

        /// <summary>
        /// ポーリング監視開始
        /// </summary>
        private async Task StartPollingMonitoringAsync()
        {
            await Task.Run(() =>
            {
                _pollingTimer = new Timer(async _ => await PollingCheckAsync(), 
                    null, TimeSpan.Zero, TimeSpan.FromSeconds(_pollingInterval));

                _logManager.LogInfo($"ポーリング監視を開始しました - 間隔: {_pollingInterval}秒");
            });
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

                await Task.Delay(1000, _cancellationTokenSource?.Token ?? CancellationToken.None);

                if (!await _fileOperationService.WaitForFileAvailableAsync(filePath))
                {
                    OnErrorOccurred($"ファイルが使用中でアクセスできません: {filePath}");
                    return;
                }

                var success = await _fileOperationService.ProcessFileAsync(filePath, _destinationFolder!, _filePrefix!);
                
                if (success)
                {
                    MarkFileAsProcessed(filePath);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"ファイル処理中にエラーが発生しました: {filePath}", ex);
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

                var patterns = FileExtensions.GetSearchPatterns(_monitorJPEG, _monitorRAW);
                var filesToProcess = new List<string>();

                foreach (var pattern in patterns)
                {
                    var files = Directory.GetFiles(_sourceFolder!, pattern, SearchOption.TopDirectoryOnly);
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
            return FileExtensions.ShouldProcessFile(filePath, _monitorJPEG, _monitorRAW);
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
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            StopMonitoringAsync().Wait();
        }
    }
}