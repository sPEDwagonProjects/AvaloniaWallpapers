using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaAlphacodersWallpaperLoader.Models;
using AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces;
using ReactiveUI;
using WallsAlphaCodersLib;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class DownloadViewModel : ViewModelBase, IView
    {
        private bool _IsVisible;
        public string Title { get; set; }

        public bool IsVisible
        {
            get => _IsVisible;
            set => this.RaiseAndSetIfChanged(ref _IsVisible, value);
        }

        public ObservableCollection<ImageModel> Images { get; set; }
        public event IView.CloseViewDelegate? CloseViewEvent;
        public WallpaperApi Api { get; set; }
        public IReactiveCommand CloseCommand { get; set; }
        public  IReactiveCommand PathClickCommand { get; set; }

        public ObservableCollection<DownloadModel>? DownloadModels { get; set; } =
            new ObservableCollection<DownloadModel>();

        public string? DownloadDirectory { get; set; }

        public DownloadViewModel(ObservableCollection<ImageModel> imageModels)
        {
            Images = imageModels;
            
            CloseCommand = ReactiveCommand.Create(() => IsVisible = false);
        }

        public async Task Load(string path)
        {
            List<DownloadModel> modelsList = new();
            foreach (var imageModel in Images)
                if (imageModel.Checked)
                {
                    try
                    {
                        string? downloadPath = CreateDownloadPath(path, imageModel);
                        var model = new DownloadModel(imageModel, downloadPath);
                        DownloadModels.Add(model);
                        modelsList.Add(model);
                    }
                    finally
                    {
                        imageModel.Checked = false;    
                    }
                }
            Task.Run(() => { modelsList.ToList().ForEach(x => { x.Download(); }); });
        }

        public string? CreateDownloadPath(string path, ImageModel? model)
        {
            int? Id = model?.Wallpaper?.Id;
            string? type = model?.Wallpaper?.File_Type;
            
            if (Id is null && type is null)
                return null;
            
            return Path.Combine(path, $"{model?.Wallpaper.Id}.{model.Wallpaper.File_Type}");
        }

        public DownloadViewModel()
        {
        }
    }
}