using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaAlphacodersWallpaperLoader.Views
{
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}