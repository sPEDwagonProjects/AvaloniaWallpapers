using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaAlphacodersWallpaperLoader.Views
{
    public partial class CategoryListControl : UserControl
    {
        public CategoryListControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}