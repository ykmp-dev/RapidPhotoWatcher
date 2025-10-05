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
        private int _currentSequenceNumber = 1;
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
                    // AppSettingsに即座に反映
                    _settings.PrefixType = value ? PrefixType.CustomText : PrefixType.DateTime;
                    OnPropertyChanged(nameof(UseDateTime));
                    OnPropertyChanged(nameof(CanEditCustomPrefix));
                    OnPropertyChanged(nameof(CanEditDateTimeFormat));
                    UpdateFileNamePreview();
                    _logManager.LogInfo($"プレフィックスタイプを {(value ? "カスタムテキスト" : "撮影日時")} に変更しました");
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
                    // AppSettingsに即座に反映
                    _settings.FilePrefix = value;
                    UpdateFileNamePreview();
                    _logManager.LogInfo($"カスタムプレフィックスを '{value}' に変更しました");
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
                    // AppSettingsに即座に反映
                    _settings.DateTimeFormatType = (DateTimeFormatType)value;
                    UpdateFileNamePreview();
                    _logManager.LogInfo($"日時フォーマットを {GetDateTimeFormatName(value)} に変更しました");
                }
            }
        }

        public List<string> SeparatorTypes { get; }

        public int SelectedSeparator
        {
            get => _selectedSeparator;
            set
            {
                // 値の範囲チェック
                if (value < 0 || value >= SeparatorTypes.Count)
                {
                    System.Diagnostics.Debug.WriteLine($"SelectedSeparator: Invalid value {value}");
                    return;
                }

                if (SetProperty(ref _selectedSeparator, value))
                {
                    try
                    {
                        // AppSettingsに即座に反映
                        _settings.SeparatorType = (SeparatorType)value;
                        UpdateFileNamePreview();
                        OnPropertyChanged(nameof(SelectedSeparatorText));
                        
                        // ログ出力（UIスレッドで実行）
                        _logManager.LogInfo($"区切り文字を {GetSeparatorName(value)} に変更しました");
                    }
                    catch (Exception ex)
                    {
                        // エラーが発生した場合はログに記録
                        System.Diagnostics.Debug.WriteLine($"SelectedSeparator error: {ex.Message}");
                    }
                }
            }
        }

        public string SelectedSeparatorText
        {
            get
            {
                if (_selectedSeparator >= 0 && _selectedSeparator < SeparatorTypes.Count)
                {
                    return SeparatorTypes[_selectedSeparator];
                }
                return "なし";
            }
        }

        public int SequenceDigits
        {
            get => _sequenceDigits;
            set
            {
                if (SetProperty(ref _sequenceDigits, value))
                {
                    // AppSettingsに即座に反映
                    _settings.SequenceDigits = value;
                    UpdateFileNamePreview();
                    _logManager.LogInfo($"連番桁数を {value} 桁に変更しました");
                }
            }
        }

        public int SequenceStartNumber
        {
            get => _currentSequenceNumber; // 現在の連番を表示
            set
            {
                if (SetProperty(ref _currentSequenceNumber, value))
                {
                    // 手動変更時は開始番号も現在の連番も同じ値に設定
                    _settings.SequenceStartNumber = value;
                    _settings.CurrentSequenceNumber = value;
                    _sequenceStartNumber = value; // 内部的な開始番号も更新
                    UpdateFileNamePreview();
                    _logManager.LogInfo($"連番を {value} に変更しました");
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
                // 連番を1に設定
                SequenceStartNumber = 1;
                _settings.ResetSequenceNumber();
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
                    CurrentSequenceNumber = _currentSequenceNumber
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
            // 設定読み込み時は即座反映を避けるため直接フィールドに設定
            _useCustomPrefix = settings.PrefixType == PrefixType.CustomText;
            _customPrefix = settings.FilePrefix ?? "IMG_";
            _selectedDateTimeFormat = (int)settings.DateTimeFormatType;
            _selectedSeparator = (int)settings.SeparatorType;
            _sequenceDigits = settings.SequenceDigits;
            _sequenceStartNumber = settings.SequenceStartNumber;
            _currentSequenceNumber = settings.CurrentSequenceNumber; // 現在の連番を設定

            // すべてのプロパティ変更を通知
            OnPropertyChanged(nameof(UseCustomPrefix));
            OnPropertyChanged(nameof(UseDateTime));
            OnPropertyChanged(nameof(CustomPrefix));
            OnPropertyChanged(nameof(SelectedDateTimeFormat));
            OnPropertyChanged(nameof(SelectedSeparator));
            OnPropertyChanged(nameof(SelectedSeparatorText));
            OnPropertyChanged(nameof(SequenceDigits));
            OnPropertyChanged(nameof(SequenceStartNumber)); // 現在の連番として表示
            OnPropertyChanged(nameof(CanEditCustomPrefix));
            OnPropertyChanged(nameof(CanEditDateTimeFormat));

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

            if (SequenceStartNumber < 0)
            {
                ShowValidationError("連番開始番号は0以上で設定してください。");
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

        private string GetSeparatorName(int index)
        {
            return index switch
            {
                0 => "なし",
                1 => "アンダーバー(_)",
                2 => "ハイフン(-)",
                _ => "不明"
            };
        }

        private string GetDateTimeFormatName(int index)
        {
            return index switch
            {
                0 => "YYYYMMDD",
                1 => "YYYY_MM_DD",
                2 => "YYYY-MM-DD",
                3 => "YYYYMMDD_HHMMSS",
                4 => "YYYY_MM_DD_HH_MM_SS",
                _ => "不明"
            };
        }

        /// <summary>
        /// AppSettingsから現在の連番を取得してViewModelを更新
        /// </summary>
        public void RefreshCurrentSequenceNumber()
        {
            _currentSequenceNumber = _settings.CurrentSequenceNumber;
            OnPropertyChanged(nameof(SequenceStartNumber)); // UIの表示を更新
            UpdateFileNamePreview();
        }

        #endregion
    }
}