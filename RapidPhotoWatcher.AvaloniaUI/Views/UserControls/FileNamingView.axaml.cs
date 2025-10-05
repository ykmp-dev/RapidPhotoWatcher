using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RapidPhotoWatcher.AvaloniaUI.Views.UserControls
{
    public partial class FileNamingView : UserControl
    {
        public FileNamingView()
        {
            InitializeComponent();
        }

        private void SeparatorButton_Click(object? sender, RoutedEventArgs e)
        {
            if (SeparatorPopup != null)
            {
                SeparatorPopup.IsOpen = !SeparatorPopup.IsOpen;
            }
        }

        private void SeparatorList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (SeparatorPopup != null)
            {
                SeparatorPopup.IsOpen = false;
            }
        }
    }
}