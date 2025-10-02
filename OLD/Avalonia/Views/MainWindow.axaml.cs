using Avalonia.Controls;
using RapidPhotoWatcher.Avalonia.ViewModels;

namespace RapidPhotoWatcher.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}