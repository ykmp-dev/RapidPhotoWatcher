using System;
using System.Threading;
using System.Windows.Forms;

namespace RapidPhotoWatcher
{
    internal static class Program
    {
        private static Mutex? _mutex = null;
        private const string MutexName = "RapidPhotoWatcher_SingleInstance";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 重複起動チェック
            _mutex = new Mutex(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                // 既に起動している場合
                MessageBox.Show("RapidPhotoWatcher は既に実行中です。", "重複起動エラー", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // スプラッシュスクリーンを表示
                SplashForm.ShowSplash(2000); // 2秒間表示
                
                // メインフォームを実行
                Application.Run(new MainForm());
            }
            finally
            {
                // アプリケーション終了時にMutexを解放
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            }
        }
    }
}