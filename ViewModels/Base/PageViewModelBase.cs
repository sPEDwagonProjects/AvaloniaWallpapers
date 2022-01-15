using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.X11;
using AvaloniaAlphacodersWallpaperLoader.Models;
using AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces;
using ReactiveUI;
using WallsAlphaCodersLib;
using WallsAlphaCodersLib.ResponseModels;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class WallpapersPageViewModelBase : ReactiveObject, IView<ImageModel>, IPagination
    {
        private static Task _task;
        
        public ObservableCollection<ImageModel> DataCollection { get; set; }
        public static Semaphore Semaphore = new Semaphore(1, 1);
        public bool IsLoading = false;
        private bool _IsVisible;
        private int _CurrentPage;
        private string _Title;
        private bool _BackPginationIsVisible = true;
        private bool _NextPaginationIsVisible = true;

        public bool BackPaginationIsVisible
        {
            get => _BackPginationIsVisible;
            set => this.RaiseAndSetIfChanged(ref _BackPginationIsVisible, value);
        }

        public bool NextPaginationIsVisible
        {
            get => _BackPginationIsVisible;
            set => this.RaiseAndSetIfChanged(ref _NextPaginationIsVisible, value);
        }

        public IReactiveCommand NextPageCommand { get; set; }
        public IReactiveCommand PreviousPageCommand { get; set; }
        public int CurrentPage { get; set; }
        public event IView.CloseViewDelegate? CloseViewEvent;
        public WallpaperApi Api { get; set; }
        public IReactiveCommand? CloseCommand { get; set; }

        public string Title
        {
            get => _Title;
            set => this.RaiseAndSetIfChanged(ref _Title, value);
        }

        public bool IsVisible
        {
            get => _IsVisible;
            set => this.RaiseAndSetIfChanged(ref _IsVisible, value);
        }


        public async Task FillEmptyData(int count) => await Task.Run(async () =>
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    DataCollection.Add(new());
                }
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return;
            }
        });

        private static CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private static CancellationToken _cancellationToken = _tokenSource.Token;

        public void ClearList() => DataCollection.Clear();

        public async Task LoadWallpapers<Tin>(Func<Tin, Task<WallpaperResponse>> func, Tin param)
        {
            if(IsLoading)
                return;
            try
            {
                IsLoading = true;
                ClearList();

                await FillEmptyData(30);

                var res = await func.Invoke(param);

                if (res.Check_Last is true)
                    NextPaginationIsVisible = false;

                for (int i = 0; i < res.Wallpapers.Count; i++)
                    DataCollection[i].Wallpaper = res.Wallpapers[i];
                

                IsLoading = false;
                await Task.Run(() =>
                {

                    int c = DataCollection.Count;
                    for (int i = 0; i < c; i++)
                        DataCollection[i].LoadBItmap();
                    

                });

            }
            catch (Exception)
            {
                ClearList();
                IsLoading = false;
            }
        }

        public WallpapersPageViewModelBase(WallpaperApi api, ObservableCollection<ImageModel> collection)
        {
            Api = api;
            CloseCommand = ReactiveCommand.Create((object obj) =>
            {
                CloseViewEvent?.Invoke();
                IsVisible = false;
            });
            NextPageCommand = ReactiveCommand.Create(() =>
            {
                CurrentPage++;
                LoadWallpapers();
            });
            PreviousPageCommand = ReactiveCommand.Create(() =>
            {
                CurrentPage++;
                LoadWallpapers();
            });
            DataCollection = collection;
        }

        public virtual Task LoadWallpapers()
        {
            throw new NotImplementedException();
        }
    }
}