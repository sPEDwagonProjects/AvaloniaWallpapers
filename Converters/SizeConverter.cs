using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaAlphacodersWallpaperLoader.Converters
{
    public class SizeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long val;
            
            if (value is int) 
                val = (long)value;
            
            else if (value is long)  
                val = (long) value;
            
            else return 0;
            
            var mb = ((double)val/1024/1024);
            
            if (mb > 1)
                return $"{mb.ToString("#.##")}МБ";
            
            var v = (double)val / 1024;
            
            return $"{v.ToString("#.##")}КБ";
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}