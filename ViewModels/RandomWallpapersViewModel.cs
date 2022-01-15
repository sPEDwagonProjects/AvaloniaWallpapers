using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AvaloniaAlphacodersWallpaperLoader.Models;
using WallsAlphaCodersLib;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class RandomWallpapersViewModel : WallpapersPageViewModelBase
    {
        
        public RandomWallpapersViewModel(WallpaperApi api, ObservableCollection<ImageModel> images) : 
            base(api,images)
        {
            BackPaginationIsVisible = false;
            NextPaginationIsVisible = false;
        }
        public  override async Task  LoadWallpapers()
        {
            try
            {
                
               await base.LoadWallpapers(Api.GetRandomWallpapers, new RandomRequestParams());    
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}