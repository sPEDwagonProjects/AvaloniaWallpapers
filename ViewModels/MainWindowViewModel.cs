using System.Collections.ObjectModel;

using Avalonia.Controls;

using AvaloniaAlphacodersWallpaperLoader.Models;
using AvaloniaAlphacodersWallpaperLoader.ViewModels.Interfaces;
using AvaloniaAlphacodersWallpaperLoader.Views;

using ReactiveUI;

using WallsAlphaCodersLib;

namespace AvaloniaAlphacodersWallpaperLoader.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private bool _NewPage = true;
        private string _Title;
        private int _SelectedMenuIndex = -1;
        private int _oldSelectedMenuIndex = -1;
        private int _ImageSelectedIndex = -1;
        private bool _ImageViewIsVisible = false;
        private IView _ActiveView;
        private IView _OldActiveView;

        private WallpaperApi api = new WallpaperApi("dcc164e06ea94f0187af2a6dfdc8bef8");

        public CategoryViewModel CategoryViewModel { get; set; }
        public RandomWallpapersViewModel RandomWallpapersViewModel { get; set; }
        public SearchViewModel SearchViewModel { get; set; }
        
        public AboutViewModel AboutViewModel{ get; set; }
        public ImageViewViewModel _ImageViewViewModel;

        public ObservableCollection<ImageModel> ImageModels { get; set; } = new ObservableCollection<ImageModel>();
        public ObservableCollection<DownloadModel> DownloadModels { get; set; } =
            new ObservableCollection<DownloadModel>();
        public DownloadViewModel DownloadViewModel{ get; set; }

        public IReactiveCommand DownloadCommand { get; set; }
        public IReactiveCommand RandomPageCommand{ get; set; }
        public IReactiveCommand CategoryPageCommand{ get; set; }
        public IReactiveCommand DownloadPageCommand{ get; set; }
        public IReactiveCommand SearchPageCommand { get; set; }
        public IReactiveCommand AboutPageCommand{ get; set; }

        public IView ActiveView
        {
            get => _ActiveView;
            set
            {
                    if(_ActiveView!=null)
                    _ActiveView.CloseViewEvent -= ActiveViewOnCloseViewEvent;
                this.RaiseAndSetIfChanged(ref _ActiveView, value);
                    _ActiveView.CloseViewEvent += ActiveViewOnCloseViewEvent;
                
            }
        }

       

        public ImageViewViewModel ImageViewViewModel
        {
            get => _ImageViewViewModel;
            set => this.RaiseAndSetIfChanged(ref _ImageViewViewModel, value);
        }

        public bool ImageViewIsVisible
        {
            get => _ImageViewIsVisible;
            set => this.RaiseAndSetIfChanged(ref _ImageViewIsVisible, value);
        }

        public int ImageSelectedIndex
        {
            get => _ImageSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _ImageSelectedIndex, value);
                if (_ImageSelectedIndex > -1)
                {
                    ImageViewViewModel = new ImageViewViewModel(ImageModels[_ImageSelectedIndex]);
                    ImageViewIsVisible = true;
                    ImageViewViewModel.CloseEvent += delegate
                    {
                        ImageViewIsVisible = false;
                        ImageViewViewModel = null;
                    };
                }
            }
        }

        public int SelectedMenuIndex
        {
            get => _SelectedMenuIndex;
            set
            {
                _SelectedMenuIndex = value;
            }
        }

        
        public MainWindowViewModel()
        {
            DownloadCommand = ReactiveCommand.Create(async () =>
                {
                    OpenFolderDialog dialog = new OpenFolderDialog();

                    var path = await dialog.ShowAsync(MainWindow.WindowInstance);

                    if (string.IsNullOrEmpty(path.Trim()) is false)
                    {
                        DownloadViewModel.Load(path.Trim());
                    }
                }
            );

            CategoryViewModel = new CategoryViewModel(api, ImageModels);
            RandomWallpapersViewModel = new RandomWallpapersViewModel(api, ImageModels);
            SearchViewModel = new SearchViewModel(api, ImageModels);
            DownloadViewModel = new DownloadViewModel(ImageModels);
            AboutViewModel = new AboutViewModel();
            ActiveView = RandomWallpapersViewModel;
            SelectedMenuIndex = 0;
            OpenView(0);

            RandomPageCommand = ReactiveCommand.Create(async () => OpenView(0));
            SearchPageCommand = ReactiveCommand.Create(async () => OpenView(1));
            CategoryPageCommand = ReactiveCommand.Create(async () =>OpenView(2));
            DownloadPageCommand = ReactiveCommand.Create(async () =>OpenView(3));
            AboutPageCommand = ReactiveCommand.Create(async () => OpenView(4));


        }

        
        private void OpenView(int index)
        {
            _oldSelectedMenuIndex = _SelectedMenuIndex;
            SelectedMenuIndex = index;
            if (ActiveView != null)
            {
                _OldActiveView = ActiveView;
            }
            if (_SelectedMenuIndex > -1)
            {
                
                ActiveView.IsVisible = false;
                switch (index)
                {
                    case 0:
                        {
                            ActiveView = RandomWallpapersViewModel;
                            RandomWallpapersViewModel.LoadWallpapers();
                            break;
                        }
                    case 1:
                        {
                            ActiveView = SearchViewModel;
                            break;
                        }
                    case 2:
                        {
                            ActiveView = CategoryViewModel;
                            break;
                        }
                    case 3:
                        {
                            ActiveView = DownloadViewModel;
                            break;
                        }
                    case 4:
                    {
                            ActiveView = AboutViewModel;
                            break;
					}
                    
                }

                ActiveView.IsVisible = true;
                
            }
        }
        private void ActiveViewOnCloseViewEvent()
        {
            ActiveView = _OldActiveView;
            ActiveView.IsVisible = true;
        }

    }
}