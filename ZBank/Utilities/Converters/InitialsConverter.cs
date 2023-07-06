using Microsoft.Toolkit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Utilities.Converters
{
    public class InitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                string[] words = str.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string initials = string.Join("", Array.ConvertAll(words, w => w[0]));
                return initials.Length > 2 ? initials.Substring(0,2)?.ToUpper() : initials;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
