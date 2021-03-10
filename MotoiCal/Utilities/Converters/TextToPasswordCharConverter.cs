using System;
using System.Globalization;
using System.Windows.Data;

namespace MotoiCal.Utilities.Converters
{
    class TextToPasswordCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new string('*', value?.ToString().Length ?? 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
