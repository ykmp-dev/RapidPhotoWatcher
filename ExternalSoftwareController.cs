using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RapidPhotoWatcher
{
    /// <summary>
    /// 外部ソフトウェア制御クラス - アクティブ化と画像選択機能
    /// </summary>
    public class ExternalSoftwareController
    {
        private readonly LogManager _logManager;
        private string? _externalSoftwarePath;
        private readonly SemaphoreSlim _activationSemaphore = new SemaphoreSlim(1, 1);


        public ExternalSoftwareController(LogManager logManager)
        {
            _logManager = logManager ?? throw new ArgumentNullException(nameof(logManager));
        }

        /// <summary>
        /// 外部ソフトウェアを起動（プレビュー表示用）
        /// </summary>
        public async Task<bool> LaunchForPreviewAsync(string softwarePath, string? targetFile = null)
        {
            await _activationSemaphore.WaitAsync();
            try
            {
                _logManager.LogInfo("外部ソフトウェア起動処理開始");
                _externalSoftwarePath = softwarePath;

                if (!File.Exists(softwarePath))
                {
                    _logManager.LogError($"外部ソフトウェアが見つかりません: {softwarePath}");
                    return false;
                }

                return await LaunchExternalSoftwareInternalAsync(softwarePath, targetFile);
            }
            catch (Exception ex)
            {
                _logManager.LogError($"外部ソフトウェア起動エラー: {ex.Message}");
                return false;
            }
            finally
            {
                _activationSemaphore.Release();
            }
        }

        /// <summary>
        /// 外部ソフトウェアを起動
        /// </summary>
        private Task<bool> LaunchExternalSoftwareInternalAsync(string softwarePath, string? targetFolder)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = softwarePath,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                // ターゲットフォルダが指定されている場合は引数として渡す
                if (!string.IsNullOrEmpty(targetFolder) && Directory.Exists(targetFolder))
                {
                    startInfo.Arguments = $"\"{targetFolder}\"";
                }

                var process = Process.Start(startInfo);
                
                if (process != null)
                {
                    _logManager.LogInfo($"外部ソフトウェアを起動しました: {Path.GetFileName(softwarePath)}");
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logManager.LogError($"外部ソフトウェア起動エラー: {ex.Message}");
                return Task.FromResult(false);
            }
        }




        /// <summary>
        /// 外部ソフトウェアが実行中かチェック
        /// </summary>
        public bool IsExternalSoftwareRunning(string softwarePath)
        {
            try
            {
                var processName = Path.GetFileNameWithoutExtension(softwarePath);
                var processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose()
        {
            _activationSemaphore?.Dispose();
        }
    }
}