using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZBank.Utilities.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return string.Empty;
            
                else if(bool.TryParse(parameter?.ToString(), out bool convert))
                {
                    if (convert && decimal.TryParse(value.ToString(), out decimal decimalValue))
                    {
                        return decimalValue.ToString("C");
                    }
                }
               else if (value is decimal)
                {
                    return ((decimal)value).ToString("C");
                }
                return value.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
