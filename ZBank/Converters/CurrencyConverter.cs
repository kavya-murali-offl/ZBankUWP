using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if((bool)parameter == true)
            {
                decimal decimalValue = decimal.Parse(value.ToString());
                if (decimalValue is decimal)
                {
                    var currencySymbol = parameter as string ?? CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
                    return ((decimal)decimalValue).ToString("C");
                }
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
