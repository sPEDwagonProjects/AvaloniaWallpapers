using ReactiveUI;
using System.Collections.ObjectModel;
using WallsAlphaCodersLib;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces
{
    public interface IView<T>:IView
    {
        public ObservableCollection<T> DataCollection { get; set; }
    }
    public interface IView
    {
        
        public delegate void CloseViewDelegate();
        public event CloseViewDelegate CloseViewEvent;
        WallpaperApi Api { get; set; }
        IReactiveCommand? CloseCommand { get; set; }
        public string Title { get; set; }
        public  bool IsVisible { get; set; }
        
        
    }
}