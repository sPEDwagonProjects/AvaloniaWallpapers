using AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces;
using ReactiveUI;

using System.Diagnostics;
using System.Runtime.InteropServices;

using WallsAlphaCodersLib;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
	public class AboutViewModel : ReactiveObject, IViewIsVisible,IView
	{
		private bool _IsVisible;

		public event IView.CloseViewDelegate CloseViewEvent;

		public bool IsVisible 
		{ 
			get => _IsVisible; 
			set => this.RaiseAndSetIfChanged(ref _IsVisible, value); 
		}

		public IReactiveCommand OpenGitHubCommand{ get; set; }
		public IReactiveCommand OpenWebCommand{ get; set; }
		public WallpaperApi Api { get; set; }
		public IReactiveCommand? CloseCommand { get; set; }
		public string Title { get; set; }

		public AboutViewModel()
		{
			
			OpenGitHubCommand = ReactiveCommand.Create((object o) => 
                                OpenUrl("https://github.com/sPEDwagonProjects/AvaloniaWallpapers"));
			OpenWebCommand = ReactiveCommand.Create((object o) => 
                                OpenUrl("https://spedwagon.online/"));

		}

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                
            }
        }
    }
}
