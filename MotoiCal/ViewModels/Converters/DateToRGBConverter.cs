﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using MotoiCal.ViewModels;

namespace MotoiCal.ViewModels.Converters
{
    class DateToRGBConverter : IValueConverter
    {
        //private readonly Brush NormalAccent = new SolidColorBrush(Color.FromRgb(0, 122, 204)); // Blue
        private readonly Brush NormalAccent = new SolidColorBrush(Colors.Gray);
        private readonly Brush SpecialAccent = new SolidColorBrush(Color.FromRgb(56, 176, 157)); // Green

        private readonly string SpecialVersion = "\"Hammertime\"";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Settings.AboutContentViewModel about = new Settings.AboutContentViewModel();

            if (about.IsEasterEggActive() && about.VersionName == this.SpecialVersion)
            {
                return this.SpecialAccent;
            }
            else
            {
                return this.NormalAccent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}