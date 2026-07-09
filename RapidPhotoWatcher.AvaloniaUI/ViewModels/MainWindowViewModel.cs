using System;
using System.Threading.Tasks;
using System.Windows.Input;
using RapidPhotoWatcher.AvaloniaUI.Commands;
using RapidPhotoWatcher.AvaloniaUI.Services;
using RapidPhotoWatcher.AvaloniaUI.Views;

namespace RapidPhotoWatcher.AvaloniaUI.ViewModels
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
                    
                    // 各子ViewModelに設定変更可能状態を通知
                    var canModify = CanModifySettings;
                    BasicSettings.UpdateCanModifySettings(canModify);
                    FileNaming.UpdateCanModifySettings(canModify);
                    ExternalSoftware.UpdateCanModifySettings(canModify);
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

        public string WindowTitle => "📷 RapidPhotoWatcher v2.2";

        #endregion

        #region Commands

        public ICommand StartMonitoringCommand { get; }
        public ICommand StopMonitoringCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 初回起動時にFTPサーバー自動セットアップ画面を表示する（Windowsのみ・一度だけ）。
        /// セットアップ成功時はFTP受信フォルダを監視フォルダとして設定する。
        /// </summary>
        public async Task OfferFtpFirstRunSetupAsync(Avalonia.Controls.Window owner)
        {
            try
            {
                if (!FtpSetupService.IsSupported || _settings.FtpOnboardingShown)
                {
                    return;
                }

                // インストーラー等で既にセットアップ済みなら表示しない
                if (FtpSetupService.IsConfigured)
                {
                    _settings.FtpOnboardingShown = true;
                    _settings.Save();
                    return;
                }

                var onboarding = new FtpOnboardingWindow();
                var ftpFolder = await onboarding.ShowDialog<string?>(owner);

                _settings.FtpOnboardingShown = true;

                if (!string.IsNullOrWhiteSpace(ftpFolder))
                {
                    if (string.IsNullOrWhiteSpace(BasicSettings.SourceFolder))
                    {
                        BasicSettings.SourceFolder = ftpFolder;
                    }
                    if (string.IsNullOrWhiteSpace(_settings.SourceFolder))
                    {
                        _settings.SourceFolder = ftpFolder;
                    }
                    Log.AddMessage($"FTPサーバーをセットアップしました（受信フォルダ: {ftpFolder}）");
                }

                _settings.Save();
            }
            catch (Exception ex)
            {
                Log.AddMessage($"FTPセットアップ画面の表示に失敗しました: {ex.Message}");
            }
        }

        private bool CanStartMonitoring() => !IsMonitoring;
        private bool CanStopMonitoring() => IsMonitoring;
        private bool CanExitApplication() => true; // 常に終了ボタンは有効だが、監視中の場合は確認メッセージを表示

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
                
                // 監視開始時に現在の連番を更新
                FileNaming.RefreshCurrentSequenceNumber();
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
                    // 監視中の場合は確認メッセージを表示
                    var shouldExit = await ShowExitConfirmationAsync();
                    if (!shouldExit)
                    {
                        return; // ユーザーがキャンセルした場合は終了しない
                    }
                    
                    Log.AddMessage("監視を停止してアプリケーションを終了します");
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

        private async Task<bool> ShowExitConfirmationAsync()
        {
            try
            {
                // 簡単なメッセージボックス実装
                // 実際のプロジェクトではMsBox.Avalonia等を使用することが推奨されます
                var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is 
                    Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                    ? desktop.MainWindow 
                    : null;

                if (mainWindow == null) return true;

                // 簡易的な確認ダイアログ（本来ならばカスタムダイアログまたはMsBox.Avaloniaを使用）
                Log.AddMessage("⚠️ 監視中にアプリケーションを終了しようとしています");
                
                // ここでは簡易実装として、常にtrueを返し、ログで警告を出力
                await Task.Delay(1000); // ユーザーに警告を読む時間を与える
                return true;
            }
            catch (Exception ex)
            {
                Log.AddMessage($"確認ダイアログエラー: {ex.Message}");
                return true; // エラーの場合は終了を許可
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
            
            // ファイル処理後に現在の連番を更新
            FileNaming.RefreshCurrentSequenceNumber();
        }

        private void OnErrorOccurred(object? sender, ErrorEventArgs e)
        {
            Log.AddMessage($"❌ エラー: {e.Message}");
            StatusMessage = "❌ エラー発生";
        }

        #endregion
    }
}