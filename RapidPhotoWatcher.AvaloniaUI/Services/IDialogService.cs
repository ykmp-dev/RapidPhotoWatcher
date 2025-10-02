using System.Threading.Tasks;

namespace RapidPhotoWatcher.AvaloniaUI.Services
{
    /// <summary>
    /// ダイアログサービスインターフェース
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// フォルダ選択ダイアログを表示
        /// </summary>
        Task<string?> ShowFolderDialogAsync(string title);

        /// <summary>
        /// ファイル選択ダイアログを表示
        /// </summary>
        Task<string?> ShowOpenFileDialogAsync(string title, string filter);

        /// <summary>
        /// 情報メッセージを表示
        /// </summary>
        Task ShowInfoAsync(string title, string message);

        /// <summary>
        /// 警告メッセージを表示
        /// </summary>
        Task ShowWarningAsync(string title, string message);

        /// <summary>
        /// エラーメッセージを表示
        /// </summary>
        Task ShowErrorAsync(string title, string message);

        /// <summary>
        /// 確認ダイアログを表示
        /// </summary>
        Task<bool> ShowConfirmAsync(string title, string message);
    }
}