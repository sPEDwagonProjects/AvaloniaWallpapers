using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaAlphacodersWallpaperLoader.Views
{
   
    public partial class MainWindow : Window
    {
        public static Window WindowInstance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            WindowInstance = this;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}