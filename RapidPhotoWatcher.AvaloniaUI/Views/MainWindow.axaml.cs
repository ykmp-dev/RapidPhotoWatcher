using Avalonia.Controls;
using Avalonia.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using RapidPhotoWatcher.AvaloniaUI.ViewModels;

namespace RapidPhotoWatcher.AvaloniaUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // 明示的にサイズを設定
            this.Width = 800;
            this.Height = 650;
            this.MinWidth = 700;
            this.MinHeight = 600;
            
            // ウィンドウのドラッグ移動を有効にする
            this.PointerPressed += OnPointerPressed;
            
            // ウィンドウ閉じるイベントを処理
            this.Closing += OnClosing;
        }

        public MainWindow(MainWindowViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
        
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // 左クリックでドラッグ移動を開始
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }

        private async void OnClosing(object? sender, WindowClosingEventArgs e)
        {
            // 監視中の場合は終了処理を ViewModelに委譲
            if (DataContext is MainWindowViewModel viewModel && viewModel.IsMonitoring)
            {
                e.Cancel = true; // 一旦キャンセル
                
                // ViewModelの終了処理を呼び出し
                if (viewModel.ExitApplicationCommand.CanExecute(null))
                {
                    viewModel.ExitApplicationCommand.Execute(null);
                }
            }
        }
    }
}