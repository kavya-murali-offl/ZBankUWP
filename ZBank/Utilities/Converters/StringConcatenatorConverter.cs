using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Utilities.Converters
{
    internal class StringConcatenatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() + " / " + parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
           throw new NotImplementedException();
        }
    }
}
