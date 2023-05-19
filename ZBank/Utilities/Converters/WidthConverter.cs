using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Utilities.Converters
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)

        {
            double width = (double)value;

            return width - 10;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string s)

        {
            throw new NotImplementedException();

        }

    }
}
