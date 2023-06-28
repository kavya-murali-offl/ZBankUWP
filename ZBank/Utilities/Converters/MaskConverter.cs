using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ZBank.Utilities.Converters
{
    internal class MaskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value == null) return "";

            string input = value as string;
            if (!string.IsNullOrEmpty(input))
            {
                return new string('*', input.Length);
            }
            return string.Empty;
            //string unMaskedString = value.ToString();
            //if(parameter is int)
            //{
            //    var maskLength = (int)parameter; 
            //    return MaskText(unMaskedString, maskLength);
            //}
            //else
            //{
            //    return new string('*', unMaskedString.Length);
            //}
        }

        private string MaskText(string unMaskedString, int maskLength)
        {
            if (unMaskedString.Length <= maskLength)
            {
                return MaskText(unMaskedString);
            }
            else
            {
                string maskedText = new string('*', maskLength);
                return maskedText + unMaskedString.Substring(maskLength);
            }
        }

        private string MaskText(string unMaskedString)
        {
            return new string('*', unMaskedString.Length);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString();
        }
    }
}
