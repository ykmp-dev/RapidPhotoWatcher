using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace RapidPhotoWatcher.AvaloniaUI.Services
{
    /// <summary>
    /// PC側FTPサーバー(IIS FTP)セットアップサービス。
    /// Setup-FtpServer.ps1 をUAC昇格付きで実行し、カメラからのFTP転送受信環境を構築する。
    /// </summary>
    public static class FtpSetupService
    {
        private const string SetupScriptName = "Setup-FtpServer.ps1";

        private static readonly string FtpConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RapidPhotoWatcher",
            "ftp-config.json");

        /// <summary>
        /// この環境でFTP自動セットアップが利用可能か(IIS FTPのためWindows限定)
        /// </summary>
        public static bool IsSupported => OperatingSystem.IsWindows();

        /// <summary>
        /// FTPセットアップ済みか(ftp-config.json の有無で判定)
        /// </summary>
        public static bool IsConfigured => File.Exists(FtpConfigFilePath);

        /// <summary>
        /// 既定のFTP受信フォルダ
        /// </summary>
        public static string DefaultFtpFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "RapidPhotoWatcher", "FTP");

        /// <summary>
        /// セットアップスクリプトを昇格実行する。
        /// </summary>
        /// <returns>プロセスの終了コード。UACキャンセルや起動失敗時は -1。</returns>
        public static async Task<int> RunSetupAsync(string userName, string password, int port, string ftpFolder)
        {
            var scriptPath = ResolveSetupScriptPath();

            var arguments =
                $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"" +
                $" -FtpRoot \"{ftpFolder}\"" +
                $" -UserName \"{userName}\"" +
                $" -Password \"{password}\"" +
                $" -Port {port} -NoPause";

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = arguments,
                    UseShellExecute = true,
                    Verb = "runas" // UAC昇格
                };

                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    return -1;
                }

                await process.WaitForExitAsync();
                return process.ExitCode;
            }
            catch (Exception)
            {
                // UACキャンセル(Win32Exception)や起動失敗
                return -1;
            }
        }

        /// <summary>
        /// スクリプトのパスを解決する。exeと同じ場所のScripts等を優先し、
        /// 見つからない場合は埋め込みリソースを一時フォルダへ展開する。
        /// </summary>
        private static string ResolveSetupScriptPath()
        {
            var baseDir = AppContext.BaseDirectory;
            var candidates = new[]
            {
                Path.Combine(baseDir, "Scripts", SetupScriptName),
                Path.Combine(baseDir, SetupScriptName),
                Path.Combine(baseDir, "Installer", SetupScriptName)
            };

            var existing = candidates.FirstOrDefault(File.Exists);
            if (existing != null)
            {
                return existing;
            }

            return ExtractEmbeddedScript();
        }

        private static string ExtractEmbeddedScript()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(SetupScriptName, StringComparison.OrdinalIgnoreCase))
                ?? throw new FileNotFoundException($"埋め込みリソース {SetupScriptName} が見つかりません。");

            var targetDir = Path.Combine(Path.GetTempPath(), "RapidPhotoWatcher");
            Directory.CreateDirectory(targetDir);
            var targetPath = Path.Combine(targetDir, SetupScriptName);

            using var resourceStream = assembly.GetManifestResourceStream(resourceName)!;
            using var fileStream = File.Create(targetPath);
            resourceStream.CopyTo(fileStream);

            return targetPath;
        }

        /// <summary>
        /// ftp-config.json からFTP受信フォルダを取得(未セットアップ時はnull)
        /// </summary>
        public static string? ReadConfiguredFtpFolder()
        {
            try
            {
                if (!File.Exists(FtpConfigFilePath))
                {
                    return null;
                }

                using var document = System.Text.Json.JsonDocument.Parse(File.ReadAllText(FtpConfigFilePath));
                if (document.RootElement.TryGetProperty("ftpFolder", out var ftpFolder))
                {
                    var folder = ftpFolder.GetString();
                    return string.IsNullOrWhiteSpace(folder) ? null : Path.GetFullPath(folder);
                }
            }
            catch
            {
                // 読めない場合は未セットアップ扱い
            }

            return null;
        }

        /// <summary>
        /// カメラに設定するPCのIPv4アドレス一覧を取得
        /// </summary>
        public static IReadOnlyList<string> GetLocalIpAddresses()
        {
            var addresses = new List<string>();
            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus != OperationalStatus.Up ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        continue;
                    }

                    foreach (var unicast in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (unicast.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            continue;
                        }

                        var ip = unicast.Address.ToString();
                        if (!ip.StartsWith("169.254.") && !addresses.Contains(ip))
                        {
                            addresses.Add(ip);
                        }
                    }
                }
            }
            catch
            {
                // ネットワーク情報が取れなくてもセットアップ自体は成立する
            }

            return addresses;
        }
    }
}
