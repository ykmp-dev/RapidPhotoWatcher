using System;
using System.Configuration;
using System.IO;
using System.Text.Json;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// 監視モード列挙型
    /// </summary>
    public enum MonitorMode
    {
        Immediate,
        Polling
    }

    /// <summary>
    /// ファイル名プレフィックスタイプ列挙型
    /// </summary>
    public enum PrefixType
    {
        CustomText,
        DateTime
    }

    /// <summary>
    /// 日時フォーマットタイプ列挙型
    /// </summary>
    public enum DateTimeFormatType
    {
        YYYYMMDD,           // 20240327
        YYYY_MM_DD,         // 2024_03_27
        YYYY_MM_DD_Hyphen,  // 2024-03-27
        YYYYMMDD_HHMMSS,    // 20240327_143052
        YYYY_MM_DD_HH_MM_SS // 2024_03_27_14_30_52
    }

    /// <summary>
    /// 接頭辞と連番の区切り文字タイプ列挙型
    /// </summary>
    public enum SeparatorType
    {
        None,       // なし
        Underscore, // アンダーバー _
        Hyphen      // ハイフン -
    }


    /// <summary>
    /// アプリケーション設定管理クラス
    /// </summary>
    public class AppSettings
    {
        private const string SettingsFileName = "settings.json";
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RapidPhotoWatcher",
            SettingsFileName);

        private int _sequenceStartNumber = 1;

        /// <summary>
        /// 監視フォルダパス
        /// </summary>
        public string? SourceFolder { get; set; }

        /// <summary>
        /// 移動先フォルダパス
        /// </summary>
        public string? DestinationFolder { get; set; }

        /// <summary>
        /// プレフィックスタイプ
        /// </summary>
        public PrefixType PrefixType { get; set; } = PrefixType.CustomText;

        /// <summary>
        /// ファイル名接頭辞（カスタムテキスト）
        /// </summary>
        public string FilePrefix { get; set; } = "IMG_";

        /// <summary>
        /// 日時フォーマットタイプ
        /// </summary>
        public DateTimeFormatType DateTimeFormatType { get; set; } = DateTimeFormatType.YYYYMMDD;

        /// <summary>
        /// 接頭辞と連番の区切り文字タイプ
        /// </summary>
        public SeparatorType SeparatorType { get; set; } = SeparatorType.None;

        /// <summary>
        /// 連番の桁数
        /// </summary>
        public int SequenceDigits { get; set; } = 4;

        /// <summary>
        /// 連番の開始番号
        /// </summary>
        public int SequenceStartNumber 
        { 
            get => _sequenceStartNumber;
            set 
            {
                if (_sequenceStartNumber != value)
                {
                    _sequenceStartNumber = value;
                    // 開始番号が変更された場合、現在の連番を新しい開始番号にリセット
                    CurrentSequenceNumber = value;
                }
            }
        }

        /// <summary>
        /// 現在の連番
        /// </summary>
        public int CurrentSequenceNumber { get; set; } = 1;

        /// <summary>
        /// 監視モード
        /// </summary>
        public MonitorMode MonitorMode { get; set; } = MonitorMode.Immediate;

        /// <summary>
        /// ポーリング間隔（秒）
        /// </summary>
        public int PollingInterval { get; set; } = 5;

        /// <summary>
        /// JPEG監視フラグ
        /// </summary>
        public bool MonitorJPEG { get; set; } = true;

        /// <summary>
        /// RAW監視フラグ
        /// </summary>
        public bool MonitorRAW { get; set; } = true;

        /// <summary>
        /// 連携ソフトウェアパス（プレビューから変更）
        /// </summary>
        public string? ExternalSoftwarePath { get; set; }

        /// <summary>
        /// 外部ソフトウェア自動アクティブ化フラグ
        /// </summary>
        public bool AutoActivateExternalSoftware { get; set; } = true;

        /// <summary>
        /// RAWファイル自動削除フラグ（JPEGのみ監視時）
        /// </summary>
        public bool AutoDeleteRawFiles { get; set; } = false;

        /// <summary>
        /// JPEGファイル自動削除フラグ（RAWのみ監視時）
        /// </summary>
        public bool AutoDeleteJpegFiles { get; set; } = false;


        /// <summary>
        /// 設定ファイルから読み込み
        /// </summary>
        public void Load()
        {
            try
            {
                // 設定ファイルが存在しない場合は新規作成
                if (!File.Exists(SettingsFilePath))
                {
                    CreateDefaultSettingsFile();
                    return;
                }

                var jsonString = File.ReadAllText(SettingsFilePath);
                
                // 空ファイルの場合はデフォルト設定で再作成
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    CreateDefaultSettingsFile();
                    return;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var settings = JsonSerializer.Deserialize<AppSettings>(jsonString, options);
                
                if (settings != null)
                {
                    // 各プロパティを個別に設定（null チェック付き）
                    SourceFolder = settings.SourceFolder ?? SourceFolder;
                    DestinationFolder = settings.DestinationFolder ?? DestinationFolder;
                    PrefixType = settings.PrefixType;
                    FilePrefix = settings.FilePrefix ?? FilePrefix;
                    DateTimeFormatType = settings.DateTimeFormatType;
                    SeparatorType = settings.SeparatorType;
                    SequenceDigits = settings.SequenceDigits > 0 ? settings.SequenceDigits : SequenceDigits;
                    
                    // 開始番号を先に設定（これによりCurrentSequenceNumberが自動調整される）
                    var loadedStartNumber = settings.SequenceStartNumber > 0 ? settings.SequenceStartNumber : SequenceStartNumber;
                    _sequenceStartNumber = loadedStartNumber; // プライベートフィールドに直接設定してセッターをバイパス
                    
                    // 現在の連番は読み込み値を使用。ただし開始番号より小さい場合は開始番号に調整
                    var loadedCurrentNumber = settings.CurrentSequenceNumber > 0 ? settings.CurrentSequenceNumber : CurrentSequenceNumber;
                    CurrentSequenceNumber = Math.Max(loadedCurrentNumber, loadedStartNumber);
                    
                    MonitorMode = settings.MonitorMode;
                    PollingInterval = settings.PollingInterval > 0 ? settings.PollingInterval : PollingInterval;
                    MonitorJPEG = settings.MonitorJPEG;
                    MonitorRAW = settings.MonitorRAW;
                    ExternalSoftwarePath = settings.ExternalSoftwarePath ?? ExternalSoftwarePath;
                    AutoActivateExternalSoftware = settings.AutoActivateExternalSoftware;
                    AutoDeleteRawFiles = settings.AutoDeleteRawFiles;
                    AutoDeleteJpegFiles = settings.AutoDeleteJpegFiles;
                }
            }
            catch (Exception ex)
            {
                // 読み込みに失敗した場合はデフォルト設定で再作成
                CreateDefaultSettingsFile();
                throw new InvalidOperationException($"設定ファイルの読み込みに失敗したため、デフォルト設定を作成しました: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// デフォルト設定ファイルを作成
        /// </summary>
        private void CreateDefaultSettingsFile()
        {
            try
            {
                // デフォルト値はプロパティ初期化子で設定済みなので、そのまま保存
                Save();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"デフォルト設定ファイルの作成に失敗しました: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 設定ファイルに保存
        /// </summary>
        public void Save()
        {
            try
            {
                var directory = Path.GetDirectoryName(SettingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var jsonString = JsonSerializer.Serialize(this, options);
                File.WriteAllText(SettingsFilePath, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"設定ファイルの保存に失敗しました: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 設定の妥当性検証
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(SourceFolder))
            {
                errorMessage = "監視フォルダが設定されていません。";
                return false;
            }

            if (!Directory.Exists(SourceFolder))
            {
                errorMessage = "監視フォルダが存在しません。";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DestinationFolder))
            {
                errorMessage = "移動先フォルダが設定されていません。";
                return false;
            }

            if (PollingInterval < 1 || PollingInterval > 3600)
            {
                errorMessage = "ポーリング間隔は1秒から3600秒の間で設定してください。";
                return false;
            }

            if (!MonitorJPEG && !MonitorRAW)
            {
                errorMessage = "監視するファイル形式を最低1つは選択してください。";
                return false;
            }

            if (SequenceStartNumber < 0)
            {
                errorMessage = "連番の開始番号は0以上で設定してください。";
                return false;
            }

            if (SequenceDigits < 1 || SequenceDigits > 10)
            {
                errorMessage = "連番の桁数は1桁から10桁の間で設定してください。";
                return false;
            }

            if (CurrentSequenceNumber < SequenceStartNumber)
            {
                errorMessage = $"現在の連番({CurrentSequenceNumber})が開始番号({SequenceStartNumber})より小さくなっています。";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 日時フォーマットに基づく文字列を生成
        /// </summary>
        public string GetDateTimePrefix(DateTime dateTime)
        {
            return DateTimeFormatType switch
            {
                DateTimeFormatType.YYYYMMDD => dateTime.ToString("yyyyMMdd"),
                DateTimeFormatType.YYYY_MM_DD => dateTime.ToString("yyyy_MM_dd"),
                DateTimeFormatType.YYYY_MM_DD_Hyphen => dateTime.ToString("yyyy-MM-dd"),
                DateTimeFormatType.YYYYMMDD_HHMMSS => dateTime.ToString("yyyyMMdd_HHmmss"),
                DateTimeFormatType.YYYY_MM_DD_HH_MM_SS => dateTime.ToString("yyyy_MM_dd_HH_mm_ss"),
                _ => dateTime.ToString("yyyyMMdd")
            };
        }

        /// <summary>
        /// 連番を取得してインクリメント
        /// </summary>
        public string GetNextSequenceNumber()
        {
            var sequenceNumber = CurrentSequenceNumber.ToString().PadLeft(SequenceDigits, '0');
            CurrentSequenceNumber++;
            return sequenceNumber;
        }

        /// <summary>
        /// 区切り文字を取得
        /// </summary>
        public string GetSeparator()
        {
            return SeparatorType switch
            {
                SeparatorType.Underscore => "_",
                SeparatorType.Hyphen => "-",
                SeparatorType.None => "",
                _ => ""
            };
        }

        /// <summary>
        /// ファイル名プレビューを生成
        /// </summary>
        public string GenerateFileNamePreview()
        {
            var dateTime = DateTime.Now;
            string prefix;
            
            if (PrefixType == PrefixType.DateTime)
            {
                prefix = GetDateTimePrefix(dateTime);
            }
            else
            {
                prefix = FilePrefix;
            }

            var separator = GetSeparator();
            var sequenceNumber = CurrentSequenceNumber.ToString().PadLeft(SequenceDigits, '0');
            
            return $"{prefix}{separator}{sequenceNumber}.jpg";
        }

        /// <summary>
        /// 連番をリセット（開始番号に戻す）
        /// </summary>
        public void ResetSequenceNumber()
        {
            CurrentSequenceNumber = SequenceStartNumber;
        }

        /// <summary>
        /// 連番を開始番号にリセットする（明示的）
        /// </summary>
        public void ResetToStartNumber()
        {
            CurrentSequenceNumber = SequenceStartNumber;
        }

        /// <summary>
        /// 開始番号を設定し、現在の連番もリセットする
        /// </summary>
        /// <param name="startNumber">新しい開始番号</param>
        public void SetStartNumberAndReset(int startNumber)
        {
            SequenceStartNumber = startNumber;
            // プロパティセッターで自動的にCurrentSequenceNumberがリセットされる
        }
    }
}