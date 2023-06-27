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
            int maskLength = 4;
            string unMaskedString = value.ToString();
            if(parameter is int)
            {
                maskLength = (int)parameter; 
            }
            return MaskText(unMaskedString, maskLength); ;
        }

        private string MaskText(string unMaskedString, int maskLength)
        {
            if (unMaskedString.Length <= maskLength)
            {
                return new string('*', unMaskedString.Length);
            }
            else
            {
                string maskedText = new string('*', maskLength);
                return maskedText + unMaskedString.Substring(maskLength);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
