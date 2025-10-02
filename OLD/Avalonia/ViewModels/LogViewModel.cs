using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using RapidPhotoWatcher.Avalonia.Commands;

namespace RapidPhotoWatcher.Avalonia.ViewModels
{
    /// <summary>
    /// ログタブのViewModel
    /// </summary>
    public class LogViewModel : ViewModelBase
    {
        private readonly LogManager _logManager;
        private string _logText = string.Empty;

        public LogViewModel(LogManager logManager)
        {
            _logManager = logManager;
            LogMessages = new ObservableCollection<LogMessage>();
            
            // コマンドを初期化
            ClearLogsCommand = new RelayCommand(ClearLogs);
            
            // 既存のログメッセージを読み込み
            LoadExistingLogs();
        }

        #region Properties

        public ObservableCollection<LogMessage> LogMessages { get; }

        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value);
        }

        public ICommand ClearLogsCommand { get; }

        #endregion

        #region Public Methods

        public void AddMessage(string message)
        {
            var logMessage = new LogMessage
            {
                Timestamp = DateTime.Now,
                Message = message,
                Level = GetLogLevel(message)
            };

            // UIスレッドで実行
            if (Avalonia.Threading.Dispatcher.UIThread.CheckAccess())
            {
                LogMessages.Add(logMessage);
                UpdateLogText();
            }
            else
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    LogMessages.Add(logMessage);
                    UpdateLogText();
                });
            }

            // LogManagerにも記録
            _logManager.LogInfo(message);
        }

        public void ClearLogs()
        {
            LogMessages.Clear();
            LogText = string.Empty;
        }

        #endregion

        #region Private Methods

        private void LoadExistingLogs()
        {
            // LogManagerから既存のログを読み込む（必要に応じて実装）
            AddMessage("📷 RapidPhotoWatcher - Avalonia Edition が起動しました");
        }

        private void UpdateLogText()
        {
            var logBuilder = new System.Text.StringBuilder();
            foreach (var message in LogMessages)
            {
                logBuilder.AppendLine($"[{message.Timestamp:HH:mm:ss}] {message.Message}");
            }
            LogText = logBuilder.ToString();
        }

        private LogLevel GetLogLevel(string message)
        {
            if (message.Contains("❌") || message.Contains("エラー"))
                return LogLevel.Error;
            if (message.Contains("⚠️") || message.Contains("警告"))
                return LogLevel.Warning;
            if (message.Contains("✅") || message.Contains("完了"))
                return LogLevel.Success;
            
            return LogLevel.Info;
        }

        #endregion
    }

    /// <summary>
    /// ログメッセージクラス
    /// </summary>
    public class LogMessage
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
        public LogLevel Level { get; set; }
        
        public string DisplayText => $"[{Timestamp:HH:mm:ss}] {Message}";
        
        public string LevelIcon => Level switch
        {
            LogLevel.Error => "❌",
            LogLevel.Warning => "⚠️",
            LogLevel.Success => "✅",
            LogLevel.Info => "ℹ️",
            _ => "📝"
        };
    }

    /// <summary>
    /// ログレベル列挙
    /// </summary>
    public enum LogLevel
    {
        Info,
        Success,
        Warning,
        Error
    }
}