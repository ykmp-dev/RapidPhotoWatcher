using System;
using System.IO;
using System.Threading.Tasks;

namespace JPEGFolderMonitor
{
    /// <summary>
    /// ファイル処理完了イベント引数
    /// </summary>
    public class FileProcessedEventArgs : EventArgs
    {
        public string OriginalPath { get; }
        public string NewPath { get; }
        public DateTime ProcessedAt { get; }

        public FileProcessedEventArgs(string originalPath, string newPath)
        {
            OriginalPath = originalPath;
            NewPath = newPath;
            ProcessedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// エラーイベント引数
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; }
        public Exception? Exception { get; }
        public DateTime OccurredAt { get; }

        public ErrorEventArgs(string message, Exception? exception = null)
        {
            Message = message;
            Exception = exception;
            OccurredAt = DateTime.Now;
        }
    }

    /// <summary>
    /// ファイル操作サービスクラス
    /// </summary>
    public class FileOperationService
    {
        private readonly LogManager _logManager;
        private int _sequenceNumber = 1;
        private readonly object _sequenceLock = new object();

        /// <summary>
        /// ファイル処理完了イベント
        /// </summary>
        public event EventHandler<FileProcessedEventArgs>? FileProcessed;

        /// <summary>
        /// エラー発生イベント
        /// </summary>
        public event EventHandler<ErrorEventArgs>? ErrorOccurred;

        public FileOperationService(LogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        /// <summary>
        /// ファイルを処理（リネーム・移動）
        /// </summary>
        /// <param name="sourceFilePath">元ファイルパス</param>
        /// <param name="destinationFolder">移動先フォルダ</param>
        /// <param name="prefix">ファイル名接頭辞</param>
        /// <returns>処理結果</returns>
        public async Task<bool> ProcessFileAsync(string sourceFilePath, string destinationFolder, string prefix)
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                {
                    var error = $"ファイルが存在しません: {sourceFilePath}";
                    OnErrorOccurred(error);
                    return false;
                }

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                    _logManager.LogInfo($"移動先フォルダを作成しました: {destinationFolder}");
                }

                var newFileName = GenerateNewFileName(sourceFilePath, prefix);
                var destinationPath = Path.Combine(destinationFolder, newFileName);

                destinationPath = await EnsureUniqueFileNameAsync(destinationPath);

                await Task.Run(() =>
                {
                    File.Move(sourceFilePath, destinationPath);
                });

                _logManager.LogInfo($"ファイル移動完了: {sourceFilePath} → {destinationPath}");
                OnFileProcessed(sourceFilePath, destinationPath);

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                var error = $"ファイルアクセス権限エラー: {sourceFilePath}";
                OnErrorOccurred(error, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                var error = $"ディレクトリが見つかりません: {destinationFolder}";
                OnErrorOccurred(error, ex);
                return false;
            }
            catch (IOException ex)
            {
                var error = $"ファイルI/Oエラー: {sourceFilePath}";
                OnErrorOccurred(error, ex);
                return false;
            }
            catch (Exception ex)
            {
                var error = $"予期しないエラー: {sourceFilePath}";
                OnErrorOccurred(error, ex);
                return false;
            }
        }

        /// <summary>
        /// 新しいファイル名を生成
        /// </summary>
        /// <param name="originalFilePath">元ファイルパス</param>
        /// <param name="prefix">接頭辞</param>
        /// <returns>新しいファイル名</returns>
        private string GenerateNewFileName(string originalFilePath, string prefix)
        {
            var extension = Path.GetExtension(originalFilePath);
            var sequence = GetNextSequenceNumber();
            
            prefix = string.IsNullOrWhiteSpace(prefix) ? "IMG_" : prefix;
            
            if (!prefix.EndsWith("_"))
            {
                prefix += "_";
            }

            return $"{prefix}{sequence:D4}{extension}";
        }

        /// <summary>
        /// 次のシーケンス番号を取得
        /// </summary>
        /// <returns>シーケンス番号</returns>
        private int GetNextSequenceNumber()
        {
            lock (_sequenceLock)
            {
                return _sequenceNumber++;
            }
        }

        /// <summary>
        /// 重複しないファイル名を確保
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>重複しないファイルパス</returns>
        private async Task<string> EnsureUniqueFileNameAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(filePath))
                {
                    return filePath;
                }

                var directory = Path.GetDirectoryName(filePath)!;
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var extension = Path.GetExtension(filePath);
                var counter = 1;

                string newPath;
                do
                {
                    var newFileName = $"{fileName}_{counter:D2}{extension}";
                    newPath = Path.Combine(directory, newFileName);
                    counter++;
                } while (File.Exists(newPath) && counter <= 999);

                if (counter > 999)
                {
                    throw new InvalidOperationException("ユニークなファイル名を生成できませんでした");
                }

                return newPath;
            });
        }

        /// <summary>
        /// ファイル使用中かどうかを確認
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>使用中の場合true</returns>
        public bool IsFileInUse(string filePath)
        {
            try
            {
                using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ファイルが完全に書き込まれるまで待機
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="maxWaitTimeSeconds">最大待機時間（秒）</param>
        /// <returns>待機が成功した場合true</returns>
        public async Task<bool> WaitForFileAvailableAsync(string filePath, int maxWaitTimeSeconds = 30)
        {
            var startTime = DateTime.Now;
            var maxWaitTime = TimeSpan.FromSeconds(maxWaitTimeSeconds);

            while (DateTime.Now - startTime < maxWaitTime)
            {
                if (!IsFileInUse(filePath))
                {
                    return true;
                }

                await Task.Delay(500);
            }

            return false;
        }

        /// <summary>
        /// シーケンス番号をリセット
        /// </summary>
        public void ResetSequenceNumber()
        {
            lock (_sequenceLock)
            {
                _sequenceNumber = 1;
            }
        }

        /// <summary>
        /// ファイル処理完了イベントを発火
        /// </summary>
        protected virtual void OnFileProcessed(string originalPath, string newPath)
        {
            FileProcessed?.Invoke(this, new FileProcessedEventArgs(originalPath, newPath));
        }

        /// <summary>
        /// エラー発生イベントを発火
        /// </summary>
        protected virtual void OnErrorOccurred(string message, Exception? exception = null)
        {
            ErrorOccurred?.Invoke(this, new ErrorEventArgs(message, exception));
        }
    }
}