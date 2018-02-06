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
    /// <summary>
    /// Returns boolean to be used with IsChecked state
    /// </summary>
    public class BoolToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool ischecked = (bool)value;            

            return ischecked;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Trims decimal and returns a max of 2 digits or 3 if 100 is reached
    /// </summary>
    public class DecimalToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double d = (double)value;
            int temp = System.Convert.ToInt32(Math.Round(d));
            return temp.ToString();
            string result = value.ToString();
            if( result.Length > 2 )
            {
                if( result.Substring(0,3) == "100" )
                {
                    result = result.Substring(0, 3);
                }
                else
                {
                    result = result.Substring(0, 2);
                }
            }                
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
