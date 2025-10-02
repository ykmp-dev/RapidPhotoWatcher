using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RapidPhotoWatcher.AvaloniaUI.Commands;

namespace RapidPhotoWatcher.AvaloniaUI.ViewModels
{
    /// <summary>
    /// 外部ソフトウェア連携タブのViewModel
    /// </summary>
    public class ExternalSoftwareViewModel : ViewModelBase
    {
        private readonly LogManager _logManager;
        
        private string _externalSoftwarePath = string.Empty;
        private bool _autoActivateExternalSoftware = true;
        private bool _canModifySettings = true;

        public ExternalSoftwareViewModel(AppSettings settings, LogManager logManager)
        {
            _logManager = logManager;
            
            BrowseExternalSoftwareCommand = new RelayCommand(BrowseExternalSoftware);
            
            ApplySettings(settings);
        }

        #region Properties

        public string ExternalSoftwarePath
        {
            get => _externalSoftwarePath;
            set => SetProperty(ref _externalSoftwarePath, value);
        }

        public bool AutoActivateExternalSoftware
        {
            get => _autoActivateExternalSoftware;
            set => SetProperty(ref _autoActivateExternalSoftware, value);
        }

        public bool CanModifySettings
        {
            get => _canModifySettings;
            set => SetProperty(ref _canModifySettings, value);
        }

        #endregion

        #region Commands

        public ICommand BrowseExternalSoftwareCommand { get; }

        #endregion

        #region Command Methods

        private async void BrowseExternalSoftware()
        {
            try
            {
                var result = await ShowFileDialogAsync();
                if (!string.IsNullOrEmpty(result))
                {
                    ExternalSoftwarePath = result;
                }
            }
            catch (Exception ex)
            {
                _logManager.LogError($"ファイル選択エラー: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task<string?> ShowFileDialogAsync()
        {
            // Avalonia固有のファイル選択実装
            var dialog = new Avalonia.Controls.OpenFileDialog
            {
                Title = "連携する外部ソフトウェアを選択してください",
                Filters = new List<Avalonia.Controls.FileDialogFilter>
                {
                    new() { Name = "実行ファイル", Extensions = { "exe" } },
                    new() { Name = "すべてのファイル", Extensions = { "*" } }
                }
            };

            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is 
                Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow 
                : null;

            var result = await dialog.ShowAsync(mainWindow);
            return result?.FirstOrDefault();
        }

        #endregion

        #region Public Methods

        public void ApplySettings(AppSettings settings)
        {
            ExternalSoftwarePath = settings.ExternalSoftwarePath ?? string.Empty;
            AutoActivateExternalSoftware = settings.AutoActivateExternalSoftware;
        }

        public void SaveToSettings(AppSettings settings)
        {
            settings.ExternalSoftwarePath = ExternalSoftwarePath;
            settings.AutoActivateExternalSoftware = AutoActivateExternalSoftware;
        }

        public bool ValidateSettings()
        {
            // 外部ソフトウェアの設定は任意なので常にtrue
            return true;
        }

        public void UpdateCanModifySettings(bool canModify)
        {
            CanModifySettings = canModify;
        }

        #endregion
    }
}