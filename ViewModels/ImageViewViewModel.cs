using Avalonia.Media.Imaging;
using System;
using System.Threading.Tasks;
using ReactiveUI;
using System.Net.Http;
using AvaloniaAlphacodersWallpaperLoader.Models;
using System.IO;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class ImageViewViewModel : ReactiveObject
    {
        public delegate void Close();

        public event Close CloseEvent;

        private Bitmap _Bitmap;
        private bool _IsLoading;

        public IReactiveCommand CloseCommand { get; set; }

        public Bitmap Image
        {
            get => _Bitmap;
            set => this.RaiseAndSetIfChanged(ref _Bitmap, value);
        }

        public ImageModel ImageModel { get; private set; }

        public bool IsLoading
        {
            get => _IsLoading;
            set => this.RaiseAndSetIfChanged(ref _IsLoading, value);
        }

        public ImageViewViewModel(ImageModel imageModel)
        {
            CloseCommand = ReactiveCommand.Create((object obj) => { CloseEvent?.Invoke(); });

            ImageModel = imageModel;

            Task.Run(() => LoadBItmap());
        }

        private async Task<MemoryStream>? GetStream()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return new MemoryStream(await client.GetByteArrayAsync(ImageModel.Wallpaper.Url_Image));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void LoadBItmap()
        {
            try
            {
                IsLoading = true;
                using (var stream = await GetStream())
                {
                    if (stream != null)
                        Image = await Task.Run(() => (new Bitmap(stream)));
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}