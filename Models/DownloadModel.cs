using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace AvaloniaAlphacodersWallpaperLoader.Models
{
    public class DownloadModel : ReactiveObject
    {
        private static readonly Semaphore Semaphore = new Semaphore(2, 2);
        private int? _Percentage;
        private long? _DownloadedSize;
        private string? _DownloadPath;
        private string? _Error;
        private bool _IsDownloaded;
        private bool _IsError;
        private ImageModel? Model { get; set; }

        public int? Percentage
        {
            get => _Percentage;
            set => this.RaiseAndSetIfChanged(ref _Percentage, value);
        }

        public bool IsError
        {
            get => _IsError;
            set => this.RaiseAndSetIfChanged(ref _IsError, value);
        }

        public bool IsDownloaded
        {
            get => _IsDownloaded;
            set => this.RaiseAndSetIfChanged(ref _IsDownloaded, value);
        }

        public string Error
        {
            get => _Error;
            set => this.RaiseAndSetIfChanged(ref _Error, value);
        }

        public string DownloadPath
        {
            get => _DownloadPath;
            set => this.RaiseAndSetIfChanged(ref _DownloadPath, value);
        }
        

        public WebClient Client { get; set; }

        public long? DownloadedSize
        {
            get => _DownloadedSize;
            set => this.RaiseAndSetIfChanged(ref _DownloadedSize, value);
        }

        public  IReactiveCommand  PathClickCommand { get; set; }
       

        public DownloadModel(ImageModel imageModel, string path)
        {
            Model = imageModel;
            DownloadPath = path;
            PathClickCommand=ReactiveCommand.Create((object o) =>
            {
                try
                {
                    Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = new FileInfo(DownloadPath).DirectoryName,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                catch (Exception)
                {
                    return;
                }


            });
        }

        public void Download()
        {
            Task.Run(async () =>
            {
                Semaphore.WaitOne();
                using (Client = new WebClient())
                {
                    Client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                    Client.DownloadFileCompleted += ClientOnDownloadFileCompleted;
                    await Client.DownloadFileTaskAsync(new Uri(Model.Wallpaper.Url_Image), DownloadPath);
                }

                Semaphore.Release();
            });
        }

        private void ClientOnDownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                IsError = true;
                Error = e.Error.Message;
            }
            else
            {
                IsDownloaded = true;
                Percentage = 100;
            }

            Client.DownloadFileCompleted -= ClientOnDownloadFileCompleted;
            Client.DownloadProgressChanged -= ClientOnDownloadProgressChanged;
        }

        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Percentage = e.ProgressPercentage;
                DownloadedSize = e.BytesReceived;
            }, DispatcherPriority.Background);
        }
    }
}