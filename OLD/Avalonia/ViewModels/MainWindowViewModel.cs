using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RapidPhotoWatcher.Avalonia.Commands;

namespace RapidPhotoWatcher.Avalonia.ViewModels
{
    /// <summary>
    /// メインウィンドウViewModel
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly FileMonitorService _fileMonitorService;
        private readonly AppSettings _settings;
        private readonly LogManager _logManager;

        private bool _isMonitoring;
        private string _statusMessage = "✅ 準備完了";
        private int _selectedTabIndex;

        public MainWindowViewModel()
        {
            _logManager = new LogManager();
            _settings = new AppSettings();
            _fileMonitorService = new FileMonitorService(_logManager);

            // 子ViewModelを初期化
            BasicSettings = new BasicSettingsViewModel(_settings, _logManager);
            FileNaming = new FileNamingViewModel(_settings, _logManager);
            ExternalSoftware = new ExternalSoftwareViewModel(_settings, _logManager);
            Log = new LogViewModel(_logManager);

            // コマンドを初期化
            StartMonitoringCommand = new AsyncRelayCommand(StartMonitoringAsync, CanStartMonitoring);
            StopMonitoringCommand = new AsyncRelayCommand(StopMonitoringAsync, CanStopMonitoring);
            ExitApplicationCommand = new AsyncRelayCommand(ExitApplicationAsync, CanExitApplication);

            // イベント購読
            _fileMonitorService.FileProcessed += OnFileProcessed;
            _fileMonitorService.ErrorOccurred += OnErrorOccurred;

            // 設定読み込み
            LoadSettings();
        }

        #region Properties

        public BasicSettingsViewModel BasicSettings { get; }
        public FileNamingViewModel FileNaming { get; }
        public ExternalSoftwareViewModel ExternalSoftware { get; }
        public LogViewModel Log { get; }

        public bool IsMonitoring
        {
            get => _isMonitoring;
            set
            {
                if (SetProperty(ref _isMonitoring, value))
                {
                    ((AsyncRelayCommand)StartMonitoringCommand).RaiseCanExecuteChanged();
                    ((AsyncRelayCommand)StopMonitoringCommand).RaiseCanExecuteChanged();
                    ((AsyncRelayCommand)ExitApplicationCommand).RaiseCanExecuteChanged();
                    OnPropertyChanged(nameof(CanModifySettings));
                }
            }
        }

        public bool CanModifySettings => !IsMonitoring;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public string WindowTitle => "📷 RapidPhotoWatcher - Avalonia Edition";

        #endregion

        #region Commands

        public ICommand StartMonitoringCommand { get; }
        public ICommand StopMonitoringCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        #endregion

        #region Command Methods

        private bool CanStartMonitoring() => !IsMonitoring;
        private bool CanStopMonitoring() => IsMonitoring;
        private bool CanExitApplication() => !IsMonitoring;

        private async Task StartMonitoringAsync()
        {
            try
            {
                if (!ValidateSettings())
                    return;

                SaveCurrentSettings();
                
                await _fileMonitorService.StartMonitoringAsync(_settings);
                
                IsMonitoring = true;
                StatusMessage = "🟢 監視中...";
                Log.AddMessage("監視を開始しました");
            }
            catch (Exception ex)
            {
                Log.AddMessage($"❌ 監視開始エラー: {ex.Message}");
                StatusMessage = "❌ エラー";
            }
        }

        private async Task StopMonitoringAsync()
        {
            try
            {
                await _fileMonitorService.StopMonitoringAsync();
                IsMonitoring = false;
                StatusMessage = "⏸️ 停止中";
                Log.AddMessage("監視を停止しました");
            }
            catch (Exception ex)
            {
                Log.AddMessage($"❌ 監視停止エラー: {ex.Message}");
                StatusMessage = "❌ エラー";
            }
        }

        private async Task ExitApplicationAsync()
        {
            try
            {
                if (IsMonitoring)
                {
                    await StopMonitoringAsync();
                }

                SaveCurrentSettings();
                _settings.Save();
                Log.AddMessage("アプリケーションを終了します");
                
                // Avalonia特有の終了処理
                if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Log.AddMessage($"❌ 終了処理エラー: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        private void LoadSettings()
        {
            try
            {
                _settings.Load();
                BasicSettings.ApplySettings(_settings);
                FileNaming.ApplySettings(_settings);
                ExternalSoftware.ApplySettings(_settings);
                Log.AddMessage("設定ファイルを読み込みました");
            }
            catch (Exception ex)
            {
                Log.AddMessage($"⚠️ 設定読み込みエラー: {ex.Message}");
            }
        }

        private bool ValidateSettings()
        {
            return BasicSettings.ValidateSettings() && 
                   FileNaming.ValidateSettings() && 
                   ExternalSoftware.ValidateSettings();
        }

        private void SaveCurrentSettings()
        {
            BasicSettings.SaveToSettings(_settings);
            FileNaming.SaveToSettings(_settings);
            ExternalSoftware.SaveToSettings(_settings);
        }

        private void OnFileProcessed(object? sender, FileProcessedEventArgs e)
        {
            var message = $"✅ {System.IO.Path.GetFileName(e.OriginalPath)} → {System.IO.Path.GetFileName(e.NewPath)}";
            Log.AddMessage(message);
            StatusMessage = "🟢 処理完了";
        }

        private void OnErrorOccurred(object? sender, ErrorEventArgs e)
        {
            Log.AddMessage($"❌ エラー: {e.Message}");
            StatusMessage = "❌ エラー発生";
        }

        #endregion
    }
}