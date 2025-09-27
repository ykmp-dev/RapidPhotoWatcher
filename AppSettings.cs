using System;
using System.Configuration;
using System.IO;
using System.Text.Json;

namespace JPEGFolderMonitor
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
    /// アプリケーション設定管理クラス
    /// </summary>
    public class AppSettings
    {
        private const string SettingsFileName = "settings.json";
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "JPEGFolderMonitor",
            SettingsFileName);

        /// <summary>
        /// 監視フォルダパス
        /// </summary>
        public string? SourceFolder { get; set; }

        /// <summary>
        /// 移動先フォルダパス
        /// </summary>
        public string? DestinationFolder { get; set; }

        /// <summary>
        /// ファイル名接頭辞
        /// </summary>
        public string FilePrefix { get; set; } = "IMG_";

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
        /// プレビューアプリケーションパス
        /// </summary>
        public string? PreviewAppPath { get; set; }

        /// <summary>
        /// 設定ファイルから読み込み
        /// </summary>
        public void Load()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                {
                    return;
                }

                var jsonString = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(jsonString);
                
                if (settings != null)
                {
                    SourceFolder = settings.SourceFolder;
                    DestinationFolder = settings.DestinationFolder;
                    FilePrefix = settings.FilePrefix;
                    MonitorMode = settings.MonitorMode;
                    PollingInterval = settings.PollingInterval;
                    MonitorJPEG = settings.MonitorJPEG;
                    MonitorRAW = settings.MonitorRAW;
                    PreviewAppPath = settings.PreviewAppPath;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"設定ファイルの読み込みに失敗しました: {ex.Message}", ex);
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

            return true;
        }
    }
}