﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;

namespace ZBank.Utilities.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case AccountType.SAVINGS:
                    return new SolidColorBrush(Colors.CadetBlue);
                case AccountType.CURRENT:
                    return new SolidColorBrush(Colors.SteelBlue);
                case AccountStatus.ACTIVE:
                    return new SolidColorBrush(Colors.SeaGreen);
                case AccountStatus.INACTIVE:
                    return new SolidColorBrush(Colors.Orange);
                case AccountStatus.CLOSED:
                    return new SolidColorBrush(Colors.IndianRed);
                case AccountType.TERM_DEPOSIT:
                    return new SolidColorBrush(Colors.RosyBrown);
                case NotificationType.SUCCESS:
                    return (AcrylicBrush)Application.Current.Resources["NotificationSuccessAcrylicBrush"];
                case NotificationType.ERROR:
                    return (AcrylicBrush)Application.Current.Resources["NotificationErrorAcrylicBrush"];
            }



            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
