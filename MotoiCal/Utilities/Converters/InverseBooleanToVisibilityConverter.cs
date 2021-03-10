using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MotoiCal.Utilities.Converters
{
    class InverseBooleanToVisibilityConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return false;
            }

            if ((Visibility)value == Visibility.Hidden)
            {
                return true;
            }

            throw new Exception(string.Format("Cannot convert back, unknown value {0}", value));
        }
    }
}
