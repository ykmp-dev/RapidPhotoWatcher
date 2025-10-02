using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using RapidPhotoWatcher.AvaloniaUI.Commands;

namespace RapidPhotoWatcher.AvaloniaUI.ViewModels
{
    /// <summary>
    /// ファイル命名タブのViewModel
    /// </summary>
    public class FileNamingViewModel : ViewModelBase
    {
        private readonly AppSettings _settings;
        private readonly LogManager _logManager;

        private bool _useCustomPrefix = true;
        private string _customPrefix = "IMG_";
        private int _selectedDateTimeFormat;
        private int _selectedSeparator;
        private int _sequenceDigits = 4;
        private int _sequenceStartNumber = 1;
        private string _fileNamePreview = "IMG_0001.jpg";
        private bool _canModifySettings = true;

        public FileNamingViewModel(AppSettings settings, LogManager logManager)
        {
            _settings = settings;
            _logManager = logManager;

            ResetSequenceCommand = new RelayCommand(ResetSequence);

            // 選択肢を初期化
            DateTimeFormats = new List<string>
            {
                "20241127 (YYYYMMDD)",
                "2024_11_27 (YYYY_MM_DD)",
                "2024-11-27 (YYYY-MM-DD)",
                "20241127_143052 (YYYYMMDD_HHMMSS)",
                "2024_11_27_14_30_52 (YYYY_MM_DD_HH_MM_SS)"
            };

            SeparatorTypes = new List<string>
            {
                "なし",
                "_(アンダーバー)",
                "-(ハイフン)"
            };

            ApplySettings(settings);
            UpdateFileNamePreview();
        }

        #region Properties

        public bool UseCustomPrefix
        {
            get => _useCustomPrefix;
            set
            {
                if (SetProperty(ref _useCustomPrefix, value))
                {
                    OnPropertyChanged(nameof(UseDateTime));
                    OnPropertyChanged(nameof(CanEditCustomPrefix));
                    OnPropertyChanged(nameof(CanEditDateTimeFormat));
                    UpdateFileNamePreview();
                }
            }
        }

        public bool UseDateTime
        {
            get => !_useCustomPrefix;
            set => UseCustomPrefix = !value;
        }

        public bool CanEditCustomPrefix => UseCustomPrefix && CanModifySettings;
        public bool CanEditDateTimeFormat => UseDateTime && CanModifySettings;

        public string CustomPrefix
        {
            get => _customPrefix;
            set
            {
                if (SetProperty(ref _customPrefix, value))
                {
                    UpdateFileNamePreview();
                }
            }
        }

        public List<string> DateTimeFormats { get; }

        public int SelectedDateTimeFormat
        {
            get => _selectedDateTimeFormat;
            set
            {
                if (SetProperty(ref _selectedDateTimeFormat, value))
                {
                    UpdateFileNamePreview();
                }
            }
        }

        public List<string> SeparatorTypes { get; }

        public int SelectedSeparator
        {
            get => _selectedSeparator;
            set
            {
                if (SetProperty(ref _selectedSeparator, value))
                {
                    UpdateFileNamePreview();
                }
            }
        }

        public int SequenceDigits
        {
            get => _sequenceDigits;
            set
            {
                if (SetProperty(ref _sequenceDigits, value))
                {
                    UpdateFileNamePreview();
                }
            }
        }

        public int SequenceStartNumber
        {
            get => _sequenceStartNumber;
            set
            {
                if (SetProperty(ref _sequenceStartNumber, value))
                {
                    UpdateFileNamePreview();
                }
            }
        }

        public string FileNamePreview
        {
            get => _fileNamePreview;
            set => SetProperty(ref _fileNamePreview, value);
        }

        public bool CanModifySettings
        {
            get => _canModifySettings;
            set => SetProperty(ref _canModifySettings, value);
        }

        #endregion

        #region Commands

        public ICommand ResetSequenceCommand { get; }

        #endregion

        #region Command Methods

        private void ResetSequence()
        {
            try
            {
                _settings.ResetSequenceNumber();
                SequenceStartNumber = 1;
                UpdateFileNamePreview();
                _logManager.LogInfo("連番を1にリセットしました");
                
                // Avalonia版メッセージボックス
                ShowInfoMessage("連番を1にリセットしました。");
            }
            catch (Exception ex)
            {
                _logManager.LogError($"連番リセットエラー: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        private void UpdateFileNamePreview()
        {
            try
            {
                var tempSettings = new AppSettings
                {
                    PrefixType = UseCustomPrefix ? PrefixType.CustomText : PrefixType.DateTime,
                    FilePrefix = CustomPrefix,
                    DateTimeFormatType = (DateTimeFormatType)SelectedDateTimeFormat,
                    SeparatorType = (SeparatorType)SelectedSeparator,
                    SequenceDigits = SequenceDigits,
                    CurrentSequenceNumber = SequenceStartNumber
                };

                FileNamePreview = tempSettings.GenerateFileNamePreview();
            }
            catch (Exception ex)
            {
                FileNamePreview = "プレビューエラー";
                _logManager.LogError($"プレビュー生成エラー: {ex.Message}");
            }
        }

        private void ShowInfoMessage(string message)
        {
            // Avalonia MessageBox実装
            _logManager.LogInfo(message);
        }

        #endregion

        #region Public Methods

        public void ApplySettings(AppSettings settings)
        {
            UseCustomPrefix = settings.PrefixType == PrefixType.CustomText;
            CustomPrefix = settings.FilePrefix ?? "IMG_";
            SelectedDateTimeFormat = (int)settings.DateTimeFormatType;
            SelectedSeparator = (int)settings.SeparatorType;
            SequenceDigits = settings.SequenceDigits;
            SequenceStartNumber = settings.CurrentSequenceNumber;

            UpdateFileNamePreview();
        }

        public void SaveToSettings(AppSettings settings)
        {
            settings.PrefixType = UseCustomPrefix ? PrefixType.CustomText : PrefixType.DateTime;
            settings.FilePrefix = CustomPrefix;
            settings.DateTimeFormatType = (DateTimeFormatType)SelectedDateTimeFormat;
            settings.SeparatorType = (SeparatorType)SelectedSeparator;
            settings.SequenceDigits = SequenceDigits;
            settings.SequenceStartNumber = SequenceStartNumber;
        }

        public bool ValidateSettings()
        {
            if (UseCustomPrefix && string.IsNullOrWhiteSpace(CustomPrefix))
            {
                ShowValidationError("カスタムプレフィックスを入力してください。");
                return false;
            }

            if (SequenceDigits < 1 || SequenceDigits > 10)
            {
                ShowValidationError("連番桁数は1〜10の範囲で設定してください。");
                return false;
            }

            return true;
        }

        public void UpdateCanModifySettings(bool canModify)
        {
            CanModifySettings = canModify;
            OnPropertyChanged(nameof(CanEditCustomPrefix));
            OnPropertyChanged(nameof(CanEditDateTimeFormat));
        }

        private void ShowValidationError(string message)
        {
            _logManager.LogError($"ファイル命名設定エラー: {message}");
        }

        #endregion
    }
}