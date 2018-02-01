using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MoodMovies.Resources.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ischecked = (bool)value;

            SolidColorBrush background = (ischecked) ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF222222")) : (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF303030"));

            return background;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
