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
                if(int.TryParse(parameter?.ToString(), out int unMaskLength))
                {
                    return MaskText(input, input.Length - unMaskLength);
                }
                return MaskText(input, input.Length);
            }
            return string.Empty;
        }

        private string MaskText(string unMaskedString, int maskLength)
        {
            StringBuilder maskedText = new StringBuilder();
            if (unMaskedString.Length == maskLength)
            {
                var individualStrings = unMaskedString.Split(' ');
                foreach (var individualString in individualStrings)
                {
                    maskedText.Append(MaskText(individualString.Length));
                    maskedText.Append(" ");
                }
                return maskedText.ToString();
            }
            else if(maskLength < unMaskedString.Length)
            {
                var individualStrings = unMaskedString.Substring(0, maskLength - 1).Split(' ');
                foreach (var individualString in individualStrings)
                {
                    maskedText.Append(MaskText(individualString.Length));
                    maskedText.Append(" ");
                }
                return maskedText.Append(unMaskedString.Substring(maskLength)).ToString();
            }
            return unMaskedString;
        }

        private string MaskText(int unMaskedStringLength)
        {
            return new string('*', unMaskedStringLength);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString();
        }
    }
}
