using System;
using System.IO;
using System.Windows.Input;
using RapidPhotoWatcher.AvaloniaUI.Commands;

namespace RapidPhotoWatcher.AvaloniaUI.ViewModels
{
    /// <summary>
    /// 基本設定タブのViewModel
    /// </summary>
    public class BasicSettingsViewModel : ViewModelBase
    {
        private readonly LogManager _logManager;
        
        private string _sourceFolder = string.Empty;
        private string _destinationFolder = string.Empty;
        private bool _monitorJpeg = true;
        private bool _monitorRaw = true;
        private bool _isImmediateMode = true;
        private int _pollingInterval = 5;
        private bool _canModifySettings = true;

        public BasicSettingsViewModel(AppSettings settings, LogManager logManager)
        {
            _logManager = logManager;
            
            BrowseSourceCommand = new RelayCommand(BrowseSourceFolder);
            BrowseDestinationCommand = new RelayCommand(BrowseDestinationFolder);
            
            ApplySettings(settings);
        }

        #region Properties

        public string SourceFolder
        {
            get => _sourceFolder;
            set => SetProperty(ref _sourceFolder, value);
        }

        public string DestinationFolder
        {
            get => _destinationFolder;
            set => SetProperty(ref _destinationFolder, value);
        }

        public bool MonitorJpeg
        {
            get => _monitorJpeg;
            set => SetProperty(ref _monitorJpeg, value);
        }

        public bool MonitorRaw
        {
            get => _monitorRaw;
            set => SetProperty(ref _monitorRaw, value);
        }

        public bool IsImmediateMode
        {
            get => _isImmediateMode;
            set
            {
                if (SetProperty(ref _isImmediateMode, value))
                {
                    OnPropertyChanged(nameof(IsPollingMode));
                    OnPropertyChanged(nameof(CanEditPollingInterval));
                }
            }
        }

        public bool IsPollingMode
        {
            get => !_isImmediateMode;
            set => IsImmediateMode = !value;
        }

        public int PollingInterval
        {
            get => _pollingInterval;
            set => SetProperty(ref _pollingInterval, value);
        }

        public bool CanModifySettings
        {
            get => _canModifySettings;
            set
            {
                if (SetProperty(ref _canModifySettings, value))
                {
                    OnPropertyChanged(nameof(CanEditPollingInterval));
                }
            }
        }

        public bool CanEditPollingInterval => IsPollingMode && CanModifySettings;

        #endregion

        #region Commands

        public ICommand BrowseSourceCommand { get; }
        public ICommand BrowseDestinationCommand { get; }

        #endregion

        #region Command Methods

        private async void BrowseSourceFolder()
        {
            try
            {
                // Avalonia フォルダ選択ダイアログ
                var result = await ShowFolderDialogAsync("監視するフォルダを選択してください");
                if (!string.IsNullOrEmpty(result))
                {
                    SourceFolder = result;
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"フォルダ選択エラー: {ex.Message}");
            }
        }

        private async void BrowseDestinationFolder()
        {
            try
            {
                var result = await ShowFolderDialogAsync("移動先フォルダを選択してください");
                if (!string.IsNullOrEmpty(result))
                {
                    DestinationFolder = result;
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"フォルダ選択エラー: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task<string?> ShowFolderDialogAsync(string title)
        {
            // Avalonia固有のフォルダ選択実装
            // 実際の実装ではAvalonia.Controls.OpenFolderDialogを使用
            var dialog = new Avalonia.Controls.OpenFolderDialog
            {
                Title = title
            };

            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is 
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow 
                : null;

            return await dialog.ShowAsync(mainWindow);
        }

        #endregion

        #region Public Methods

        public void ApplySettings(AppSettings settings)
        {
            SourceFolder = settings.SourceFolder ?? string.Empty;
            DestinationFolder = settings.DestinationFolder ?? string.Empty;
            MonitorJpeg = settings.MonitorJPEG;
            MonitorRaw = settings.MonitorRAW;
            IsImmediateMode = settings.MonitorMode == MonitorMode.Immediate;
            PollingInterval = settings.PollingInterval;
        }

        public void SaveToSettings(AppSettings settings)
        {
            settings.SourceFolder = SourceFolder;
            settings.DestinationFolder = DestinationFolder;
            settings.MonitorJPEG = MonitorJpeg;
            settings.MonitorRAW = MonitorRaw;
            settings.MonitorMode = IsImmediateMode ? MonitorMode.Immediate : MonitorMode.Polling;
            settings.PollingInterval = PollingInterval;
        }

        public bool ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(SourceFolder))
            {
                ShowValidationError("監視フォルダを選択してください。");
                return false;
            }

            if (!Directory.Exists(SourceFolder))
            {
                ShowValidationError("監視フォルダが存在しません。");
                return false;
            }

            if (string.IsNullOrWhiteSpace(DestinationFolder))
            {
                ShowValidationError("移動先フォルダを選択してください。");
                return false;
            }

            if (!MonitorJpeg && !MonitorRaw)
            {
                ShowValidationError("監視するファイル形式を選択してください。");
                return false;
            }

            return true;
        }

        public void UpdateCanModifySettings(bool canModify)
        {
            CanModifySettings = canModify;
        }

        private void ShowValidationError(string message)
        {
            // Avalonia MessageBox実装
            _logManager.LogError($"設定検証エラー: {message}");
        }

        #endregion
    }
}