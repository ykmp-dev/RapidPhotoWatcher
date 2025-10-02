using System;
using System.Diagnostics;
using System.IO;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// プレビューアプリケーション連携サービス
    /// </summary>
    public class PreviewService
    {
        private readonly string _previewAppPath;

        public PreviewService(string previewAppPath)
        {
            _previewAppPath = previewAppPath ?? throw new ArgumentNullException(nameof(previewAppPath));
            
            if (!File.Exists(_previewAppPath))
            {
                throw new FileNotFoundException($"プレビューアプリケーションが見つかりません: {_previewAppPath}");
            }
        }

        /// <summary>
        /// 指定したファイルをプレビューアプリで開く
        /// </summary>
        /// <param name="filePath">開くファイルのパス</param>
        /// <returns>プロセス開始が成功した場合true</returns>
        public bool OpenFile(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("ファイルパスが指定されていません", nameof(filePath));
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"ファイルが見つかりません: {filePath}");
                }

                if (!FileExtensions.IsSupportedFile(filePath))
                {
                    throw new ArgumentException($"サポートされていないファイル形式です: {filePath}");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = _previewAppPath,
                    Arguments = $"\"{filePath}\"",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                Console.WriteLine($"[DEBUG] プロセス起動準備: FileName='{startInfo.FileName}', Arguments='{startInfo.Arguments}'");
                
                var process = Process.Start(startInfo);
                
                var success = process != null;
                Console.WriteLine($"[DEBUG] プロセス起動結果: {success}");
                
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] プロセス起動例外: {ex.Message}");
                throw new InvalidOperationException($"プレビューアプリの起動に失敗しました: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// フォルダ内の最初のサポート対象ファイルをプレビューで開く
        /// </summary>
        /// <param name="folderPath">フォルダパス</param>
        /// <param name="monitorJPEG">JPEG監視フラグ</param>
        /// <param name="monitorRAW">RAW監視フラグ</param>
        /// <returns>ファイルが見つかり、開かれた場合true</returns>
        public bool OpenFirstSupportedFile(string folderPath, bool monitorJPEG, bool monitorRAW)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    throw new ArgumentException("フォルダパスが指定されていません", nameof(folderPath));
                }

                if (!Directory.Exists(folderPath))
                {
                    throw new DirectoryNotFoundException($"フォルダが見つかりません: {folderPath}");
                }

                var patterns = FileExtensions.GetSearchPatterns(monitorJPEG, monitorRAW);
                
                foreach (var pattern in patterns)
                {
                    var files = Directory.GetFiles(folderPath, pattern, SearchOption.TopDirectoryOnly);
                    
                    if (files.Length > 0)
                    {
                        Array.Sort(files, StringComparer.OrdinalIgnoreCase);
                        return OpenFile(files[0]);
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"フォルダ内ファイルのプレビューに失敗しました: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// プレビューアプリケーションが有効かどうかを確認
        /// </summary>
        /// <returns>有効な場合true</returns>
        public bool IsValid()
        {
            return File.Exists(_previewAppPath);
        }

        /// <summary>
        /// プレビューアプリケーションの情報を取得
        /// </summary>
        /// <returns>アプリケーション情報</returns>
        public string GetApplicationInfo()
        {
            try
            {
                if (!File.Exists(_previewAppPath))
                {
                    return "プレビューアプリケーションが見つかりません";
                }

                var fileInfo = new FileInfo(_previewAppPath);
                var versionInfo = FileVersionInfo.GetVersionInfo(_previewAppPath);

                return $"{versionInfo.FileDescription ?? Path.GetFileNameWithoutExtension(_previewAppPath)} " +
                       $"(バージョン: {versionInfo.FileVersion ?? "不明"})";
            }
            catch (Exception ex)
            {
                return $"アプリケーション情報の取得に失敗しました: {ex.Message}";
            }
        }

        /// <summary>
        /// デフォルトアプリでファイルを開く（プレビューアプリが設定されていない場合のフォールバック）
        /// </summary>
        /// <param name="filePath">開くファイルのパス</param>
        /// <returns>プロセス開始が成功した場合true</returns>
        public static bool OpenWithDefaultApp(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("ファイルパスが指定されていません", nameof(filePath));
                }

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"ファイルが見つかりません: {filePath}");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };

                var process = Process.Start(startInfo);
                
                return process != null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"デフォルトアプリでのファイル起動に失敗しました: {ex.Message}", ex);
            }
        }
    }
}