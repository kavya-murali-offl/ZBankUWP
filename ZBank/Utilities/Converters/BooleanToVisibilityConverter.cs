﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace ZBank.Utilities.Converters
{
        public class BooleanToVisibilityConverter : IValueConverter
        {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is bool boolValue)
            {
                if(parameter != null && bool.TryParse(parameter.ToString(), out bool invert))
                {
                    return boolValue? Visibility.Collapsed: Visibility.Visible; ;
                }
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if(value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
