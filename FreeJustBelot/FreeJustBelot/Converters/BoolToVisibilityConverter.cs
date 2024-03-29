﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FreeJustBelot.Converters
{
    public class BoolToVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                return null;
            }
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            var isVisible = bool.Parse(value.ToString());

            return (isVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
