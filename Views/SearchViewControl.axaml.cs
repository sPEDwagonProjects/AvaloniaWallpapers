using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaAlphacodersWallpaperLoader.Views
{
    public partial class SearchViewControl : UserControl
    {
        public SearchViewControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}