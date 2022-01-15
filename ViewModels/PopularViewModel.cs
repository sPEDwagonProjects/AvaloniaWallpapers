using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using AvaloniaAlphacodersWallpaperLoader.Models;
using WallsAlphaCodersLib;
using WallsAlphaCodersLib.Popular;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class PopularViewModels:WallpapersPageViewModelBase
    {
        public PopularViewModels( WallpaperApi api,  ObservableCollection<ImageModel> collection) : 
            base(api, collection)
        {
                
        }
        
        public  override async Task  LoadWallpapers()
        {
            try
            {
                await base.LoadWallpapers(Api.Popular.GetPopular, new PopularRequestParams());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}