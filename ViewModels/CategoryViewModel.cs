using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using ReactiveUI;
using WallsAlphaCodersLib;
using WallsAlphaCodersLib.Category;
using WallsAlphaCodersLib.ResponseModels.Data;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class CategoryViewModel : WallpapersPageViewModelBase
    {
        private int _CategorySelectedIndex;
        public ObservableCollection<Category> Categories { get; set; } = new();

        public int CategorySelectedIndex
        {
            get => _CategorySelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _CategorySelectedIndex, value);
                if (_CategorySelectedIndex > -1)
                {
                    CurrentPage = 1;
                    IsVisible = false;

                    Title = $"Категория: {Categories[_CategorySelectedIndex].Name}";
                    LoadWallpapers();
                }
            }
        }

        public CategoryViewModel(WallpaperApi api, ObservableCollection<Models.ImageModel> imageModels) : base(api,
            imageModels)
        {
            CurrentPage = 1;
            var resAwaiter = Api.Category.GetCategoryList().GetAwaiter();
            resAwaiter.OnCompleted(() => { resAwaiter.GetResult().Items.ForEach(x => Categories.Add(x)); });
        }

        public override async Task LoadWallpapers()
        {
            try
            {
               

                var param = new CategoryRequestParams()
                {
                    Id = (int)Categories[CategorySelectedIndex].Id,
                    Page = CurrentPage,
                    Check_Last = true,
                   

                };
               await base.LoadWallpapers(Api.Category.GetCategory, param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}