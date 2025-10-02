using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RapidPhotoWatcher.Avalonia.Views;
using RapidPhotoWatcher.Avalonia.ViewModels;
using RapidPhotoWatcher.Avalonia.Services;

namespace RapidPhotoWatcher.Avalonia
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
                // メインウィンドウとViewModelを設定
                var viewModel = new MainWindowViewModel();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = viewModel
                };

                // アプリケーション終了時の処理
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}