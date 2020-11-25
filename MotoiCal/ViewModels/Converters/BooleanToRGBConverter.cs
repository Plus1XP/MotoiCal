using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MotoiCal.ViewModels.Converters
{
    class BooleanToRGBConverter : IValueConverter
    {
        private readonly Brush AccentActive = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private readonly Brush AccentInactive = new SolidColorBrush(Color.FromRgb(105, 105, 105));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? this.AccentActive : this.AccentInactive;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Brush)value == this.AccentActive)
            {
                return true;
            }

            if ((Brush)value == this.AccentInactive)
            {
                return false;
            }

            throw new Exception(string.Format("Cannot convert back, unknown value {0}", value));
        }
    }
}
