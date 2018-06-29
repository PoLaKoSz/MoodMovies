using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MoodMovies.Resources.Converters
{
    /// <summary>
    /// converts boolean value to a specific color
    /// </summary>
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
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
