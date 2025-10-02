using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace RapidPhotoWatcher.AvaloniaUI
{
    internal class Program
    {
        private static Mutex? _mutex = null;
        private const string MutexName = "RapidPhotoWatcher_Avalonia_SingleInstance";

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // 重複起動チェック
            _mutex = new Mutex(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                // 既に起動している場合
                Console.WriteLine("RapidPhotoWatcher - Avalonia Edition は既に実行中です。");
                return;
            }

            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            finally
            {
                // アプリケーション終了時にMutexを解放
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}