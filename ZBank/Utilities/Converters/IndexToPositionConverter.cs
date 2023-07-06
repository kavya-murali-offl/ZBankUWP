using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Utilities.Converters
{
    internal class IndexToPositionConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue)
            {
                return intValue == 0 ? "1" : (intValue + 1).ToString();
            }
            else if (value is double doubleValue)
            {
                return doubleValue == 0 ? "1" : (doubleValue + 1).ToString();
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
