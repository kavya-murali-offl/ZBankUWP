using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using ZBank.Entities.EnumerationType;

namespace ZBank.Utilities.Converters
{
    internal class StringEqualityConverter : IValueConverter
    {

            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return value?.ToString()?.Equals(parameter?.ToString());
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                var parameterString = parameter as string;
                var valueAsBool = (bool)value;

                if (parameterString == null || !valueAsBool)
                {
                    try
                    {
                        return Enum.Parse(targetType, "0");
                    }
                    catch (Exception)
                    {
                        return DependencyProperty.UnsetValue;
                    }
                }
                return Enum.Parse(targetType, parameterString);

        }
    }
}
