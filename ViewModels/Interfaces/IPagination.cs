using ReactiveUI;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces
{
    public interface IPagination : IPaginationVisible
    {
        public IReactiveCommand NextPageCommand { get; set; }
        public IReactiveCommand PreviousPageCommand { get; set; }
        public int CurrentPage { get; set; }
        
    }
}