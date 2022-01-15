using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using JetBrains.Annotations;
using WallsAlphaCodersLib.ResponseModels.Data;

namespace AvaloniaAlphacodersWallpaperLoader.Models
{
	public class ImageModel : INotifyPropertyChanged,IModelBase<Wallpaper?>
	{
		
		private Bitmap _bitmap;
		private bool _Checked;

		public bool Checked
		{
			get => _Checked;
			set
			{
				_Checked = value;
				OnPropertyChanged();
			}
		}
		
		public  Wallpaper Wallpaper { get; set; }

		public Bitmap BitmapImage
		{
			get => _bitmap;
			set
			{
				_bitmap = value;
				OnPropertyChanged();
			}
		}
		
		private async Task<MemoryStream>? GetStream()
		{
			try
			{
				using (HttpClient client = new HttpClient())
					return new MemoryStream(await client.GetByteArrayAsync(Wallpaper.Url_Thumb));
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

			
					
					using (var stream = await GetStream())
						if (stream != null)
							BitmapImage = await Task.Run(() => Bitmap.DecodeToWidth(stream, 600,
								Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.LowQuality));
					
				
				
			}
			catch (Exception EX)
			{
				return;
			}
		}
		
		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			try
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
			catch (ArgumentException)
			{
				return;
			}



		}

		public void SetData(Wallpaper? obj)
		{
			Wallpaper = obj;
		}
	}
}