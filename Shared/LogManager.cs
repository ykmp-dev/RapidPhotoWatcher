using System;
using System.IO;
using System.Text;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// ログレベル列挙型
    /// </summary>
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// ログ管理クラス
    /// </summary>
    public class LogManager
    {
        private const string LogFileName = "application.log";
        private static readonly string LogFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RapidPhotoWatcher",
            "Logs",
            LogFileName);

        private readonly object _lockObject = new object();

        public LogManager()
        {
            EnsureLogDirectoryExists();
        }

        /// <summary>
        /// ログディレクトリの存在確認と作成
        /// </summary>
        private void EnsureLogDirectoryExists()
        {
            var directory = Path.GetDirectoryName(LogFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// 情報ログの記録
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void LogInfo(string message)
        {
            WriteLog(LogLevel.Info, message);
        }

        /// <summary>
        /// 警告ログの記録
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void LogWarning(string message)
        {
            WriteLog(LogLevel.Warning, message);
        }

        /// <summary>
        /// エラーログの記録
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void LogError(string message)
        {
            WriteLog(LogLevel.Error, message);
        }

        /// <summary>
        /// 例外ログの記録
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="exception">例外オブジェクト</param>
        public void LogError(string message, Exception exception)
        {
            var fullMessage = $"{message}\n例外詳細: {exception}";
            WriteLog(LogLevel.Error, fullMessage);
        }

        /// <summary>
        /// ログファイルへの書き込み
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="message">メッセージ</param>
        private void WriteLog(LogLevel level, string message)
        {
            try
            {
                lock (_lockObject)
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var logEntry = $"[{timestamp}] [{level}] {message}";

                    using var writer = new StreamWriter(LogFilePath, append: true, Encoding.UTF8);
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ログ書き込みエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// ログファイルのクリア
        /// </summary>
        public void ClearLog()
        {
            try
            {
                lock (_lockObject)
                {
                    if (File.Exists(LogFilePath))
                    {
                        File.Delete(LogFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(LogLevel.Error, $"ログファイルクリアエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// ログファイルの内容取得
        /// </summary>
        /// <returns>ログファイルの内容</returns>
        public string GetLogContent()
        {
            try
            {
                lock (_lockObject)
                {
                    if (!File.Exists(LogFilePath))
                    {
                        return string.Empty;
                    }

                    return File.ReadAllText(LogFilePath, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                return $"ログファイル読み込みエラー: {ex.Message}";
            }
        }

        /// <summary>
        /// 古いログファイルのローテーション
        /// </summary>
        /// <param name="maxFileSizeInMB">最大ファイルサイズ（MB）</param>
        public void RotateLogIfNeeded(int maxFileSizeInMB = 10)
        {
            try
            {
                if (!File.Exists(LogFilePath))
                {
                    return;
                }

                var fileInfo = new FileInfo(LogFilePath);
                var maxSizeInBytes = maxFileSizeInMB * 1024 * 1024;

                if (fileInfo.Length > maxSizeInBytes)
                {
                    var backupPath = LogFilePath.Replace(".log", $"_{DateTime.Now:yyyyMMdd_HHmmss}.log");
                    File.Move(LogFilePath, backupPath);
                    
                    WriteLog(LogLevel.Info, $"ログファイルをローテーションしました: {backupPath}");
                }
            }
            catch (Exception ex)
            {
                WriteLog(LogLevel.Error, $"ログローテーションエラー: {ex.Message}");
            }
        }
    }
}