using System;
using System.Globalization;
//using System.Drawing;
using Xamarin.Forms;

namespace IdentifyMe.App.Converters
{
    public class NameToColorConverter : IValueConverter
    {
        static Random random = new Random(DateTime.Now.Second);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Color.FromRgb(random.Next(1,256), 
                random.Next(1, 256), 
                random.Next(1, 256));
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
