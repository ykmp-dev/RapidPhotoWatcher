using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using RapidPhotoWatcher.AvaloniaUI.Services;

namespace RapidPhotoWatcher.AvaloniaUI.Views
{
    /// <summary>
    /// 初回起動時のFTPサーバー自動セットアップ画面。
    /// ShowDialog の戻り値として、成功時はFTP受信フォルダのパス、スキップ/失敗時は null を返す。
    /// </summary>
    public partial class FtpOnboardingWindow : Window
    {
        public FtpOnboardingWindow()
        {
            InitializeComponent();
            FolderBox.Text = FtpSetupService.DefaultFtpFolder;
        }

        private void OnSkipClick(object? sender, RoutedEventArgs e)
        {
            Close(null);
        }

        private async void OnSetupClick(object? sender, RoutedEventArgs e)
        {
            var userName = (UserNameBox.Text ?? string.Empty).Trim();
            var password = PasswordBox.Text ?? string.Empty;
            var folder = (FolderBox.Text ?? string.Empty).Trim();

            if (userName.Length == 0)
            {
                ShowStatus("FTPユーザー名を入力してください。", isError: true);
                return;
            }
            if (password.Length == 0)
            {
                ShowStatus("FTPパスワードを入力してください。", isError: true);
                return;
            }
            if (userName.Contains('"') || password.Contains('"'))
            {
                ShowStatus("ユーザー名とパスワードにダブルクォート ( \" ) は使用できません。", isError: true);
                return;
            }
            if (!int.TryParse((PortBox.Text ?? string.Empty).Trim(), out var port) || port < 1 || port > 65535)
            {
                ShowStatus("ポート番号は 1～65535 の数値で入力してください。", isError: true);
                return;
            }
            if (folder.Length == 0)
            {
                ShowStatus("FTP受信フォルダを入力してください。", isError: true);
                return;
            }

            SetupButton.IsEnabled = false;
            SkipButton.IsEnabled = false;
            ShowStatus("セットアップを実行しています... (UACの確認が表示されます)", isError: false);

            var exitCode = await FtpSetupService.RunSetupAsync(userName, password, port, folder);

            if (exitCode == 0)
            {
                var ipList = FtpSetupService.GetLocalIpAddresses();
                var ipText = ipList.Any() ? string.Join(" / ", ipList) : "(PCのIPアドレスを確認してください)";

                await Services.MessageBoxManager.GetMessageBoxStandard(
                    "FTPサーバーのセットアップ完了",
                    "FTPサーバーの準備ができました！\n\n" +
                    "カメラ側には以下を設定してください:\n" +
                    $"  サーバー(IPアドレス): {ipText}\n" +
                    $"  ポート: {port}\n" +
                    $"  ユーザー名: {userName}\n" +
                    "  パスワード: (入力したパスワード)\n" +
                    "  転送モード: パッシブ(PASV)推奨\n\n" +
                    $"FTP受信フォルダ:\n  {folder}\n" +
                    "このフォルダを監視フォルダとして自動設定しました。",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Success);

                Close(folder);
                return;
            }

            SetupButton.IsEnabled = true;
            SkipButton.IsEnabled = true;
            ShowStatus(
                "セットアップに失敗またはキャンセルされました。" +
                "再試行するか、「あとで設定する」で閉じてください。" +
                $"(詳細ログ: %TEMP%\\RapidPhotoWatcher-FtpSetup.log)",
                isError: true);
        }

        private void ShowStatus(string message, bool isError)
        {
            StatusText.Text = message;
            StatusText.Foreground = isError
                ? Avalonia.Media.Brushes.OrangeRed
                : Avalonia.Media.Brushes.Gray;
            StatusText.IsVisible = true;
        }
    }
}
