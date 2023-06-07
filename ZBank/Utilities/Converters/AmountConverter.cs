using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using ZBank.Entities;

namespace ZBank.Utilities.Converters
{
    public class AmountConverter : DependencyObject, IValueConverter
    {

        public TransactionType TransactionType
        {
            get { return (TransactionType)GetValue(TransactionTypeProperty); }
            set { SetValue(TransactionTypeProperty, value); }
        }

        public static readonly DependencyProperty TransactionTypeProperty =
        DependencyProperty.Register("TransactionType",
                                    typeof(TransactionType),
                                    typeof(AmountConverter),
                                    new PropertyMetadata(null));


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if(TransactionType == TransactionType.DEBIT)
            {
                return "+" + value;
            }
            else
            {
                return "-" + value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}
