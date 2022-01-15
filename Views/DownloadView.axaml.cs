using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaAlphacodersWallpaperLoader.Views
{
    public partial class DownloadView : UserControl
    {
        public DownloadView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}