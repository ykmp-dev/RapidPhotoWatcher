using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RapidPhotoWatcher.AvaloniaUI.Views;
using RapidPhotoWatcher.AvaloniaUI.ViewModels;
using RapidPhotoWatcher.AvaloniaUI.Services;

namespace RapidPhotoWatcher.AvaloniaUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // サービスコンテナを初期化
            ServiceContainer.Initialize();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // スプラッシュウィンドウを最初に表示
                var splashWindow = new SplashWindow();
                
                splashWindow.SplashCompleted += () =>
                {
                    // スプラッシュ完了後にメインウィンドウを表示
                    var viewModel = new MainWindowViewModel();
                    var mainWindow = new MainWindow
                    {
                        DataContext = viewModel
                    };
                    
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                };
                
                // スプラッシュウィンドウを表示
                splashWindow.Show();

                // アプリケーション終了時の処理
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}