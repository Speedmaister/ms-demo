using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FreeJustBelot.Converters
{
    public class BoolToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Brush))
            {
                return null;
            }
            if (value == null)
            {
                return new SolidColorBrush(Windows.UI.Colors.Red);
            }
            var isOnline = bool.Parse(value.ToString());

            return (isOnline) ? new SolidColorBrush(Windows.UI.Colors.GreenYellow) : new SolidColorBrush(Windows.UI.Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
