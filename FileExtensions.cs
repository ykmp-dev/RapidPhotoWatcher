using System;
using System.Collections.Generic;
using System.Linq;

namespace JPEGFolderMonitor
{
    /// <summary>
    /// ファイル拡張子関連のユーティリティクラス
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// JPEG形式の拡張子リスト
        /// </summary>
        private static readonly HashSet<string> JpegExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg"
        };

        /// <summary>
        /// RAW形式の拡張子リスト
        /// </summary>
        private static readonly HashSet<string> RawExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".cr2",   // Canon RAW Version 2
            ".cr3",   // Canon RAW Version 3
            ".arw",   // Sony Alpha Raw
            ".nef",   // Nikon Electronic Format
            ".rw2",   // Panasonic RAW Version 2
            ".orf",   // Olympus RAW Format
            ".dng"    // Digital Negative (Adobe)
        };

        /// <summary>
        /// すべてのサポート対象拡張子リスト
        /// </summary>
        public static IEnumerable<string> AllSupportedExtensions => 
            JpegExtensions.Union(RawExtensions);

        /// <summary>
        /// 指定された拡張子がJPEG形式かどうかを判定
        /// </summary>
        /// <param name="extension">拡張子（ドット付き）</param>
        /// <returns>JPEG形式の場合true</returns>
        public static bool IsJPEG(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            return JpegExtensions.Contains(extension);
        }

        /// <summary>
        /// 指定された拡張子がRAW形式かどうかを判定
        /// </summary>
        /// <param name="extension">拡張子（ドット付き）</param>
        /// <returns>RAW形式の場合true</returns>
        public static bool IsRAW(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            return RawExtensions.Contains(extension);
        }

        /// <summary>
        /// 指定された拡張子がサポート対象かどうかを判定
        /// </summary>
        /// <param name="extension">拡張子（ドット付き）</param>
        /// <returns>サポート対象の場合true</returns>
        public static bool IsSupported(string extension)
        {
            return IsJPEG(extension) || IsRAW(extension);
        }

        /// <summary>
        /// ファイルパスから拡張子を取得してサポート対象かどうかを判定
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>サポート対象の場合true</returns>
        public static bool IsSupportedFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            var extension = System.IO.Path.GetExtension(filePath);
            return IsSupported(extension);
        }

        /// <summary>
        /// ファイルパスから拡張子を取得してJPEGかどうかを判定
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>JPEG形式の場合true</returns>
        public static bool IsJPEGFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            var extension = System.IO.Path.GetExtension(filePath);
            return IsJPEG(extension);
        }

        /// <summary>
        /// ファイルパスから拡張子を取得してRAWかどうかを判定
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>RAW形式の場合true</returns>
        public static bool IsRAWFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            var extension = System.IO.Path.GetExtension(filePath);
            return IsRAW(extension);
        }

        /// <summary>
        /// 監視設定に基づいてファイルが処理対象かどうかを判定
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="monitorJPEG">JPEG監視フラグ</param>
        /// <param name="monitorRAW">RAW監視フラグ</param>
        /// <returns>処理対象の場合true</returns>
        public static bool ShouldProcessFile(string filePath, bool monitorJPEG, bool monitorRAW)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            if (monitorJPEG && IsJPEGFile(filePath))
                return true;

            if (monitorRAW && IsRAWFile(filePath))
                return true;

            return false;
        }

        /// <summary>
        /// 拡張子の表示名を取得
        /// </summary>
        /// <param name="extension">拡張子（ドット付き）</param>
        /// <returns>表示名</returns>
        public static string GetDisplayName(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return "不明";

            extension = extension.ToUpperInvariant();

            return extension switch
            {
                ".JPG" or ".JPEG" => "JPEG画像",
                ".CR2" => "Canon RAW (CR2)",
                ".CR3" => "Canon RAW (CR3)",
                ".ARW" => "Sony Alpha RAW",
                ".NEF" => "Nikon RAW",
                ".RW2" => "Panasonic RAW",
                ".ORF" => "Olympus RAW",
                ".DNG" => "Adobe DNG RAW",
                _ => extension.TrimStart('.')
            };
        }

        /// <summary>
        /// ファイル検索パターンを取得
        /// </summary>
        /// <param name="monitorJPEG">JPEG監視フラグ</param>
        /// <param name="monitorRAW">RAW監視フラグ</param>
        /// <returns>検索パターンの配列</returns>
        public static string[] GetSearchPatterns(bool monitorJPEG, bool monitorRAW)
        {
            var patterns = new List<string>();

            if (monitorJPEG)
            {
                patterns.AddRange(JpegExtensions.Select(ext => $"*{ext}"));
            }

            if (monitorRAW)
            {
                patterns.AddRange(RawExtensions.Select(ext => $"*{ext}"));
            }

            return patterns.ToArray();
        }
    }
}