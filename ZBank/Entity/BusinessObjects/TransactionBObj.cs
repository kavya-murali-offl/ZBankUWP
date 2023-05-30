using System;
using Windows.UI.Xaml.Media;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj : Transaction
    {
        public string Name { get; set; }

        public string ArrowIcon { get; set; }

        public string BorderColor { get;  set; }

        public string BackgroundColor { get;  set; }

        public void SetDefault()
        {
            if (TransactionType == TransactionType.EXPENSE)
            {
                BorderColor = "#be3232";
                BackgroundColor = "#f5e1dd";
                ArrowIcon = "\uEDDC";
            }
            else if (TransactionType == TransactionType.INCOME)
            {
                BackgroundColor = "#eafde8";
                BorderColor = "#058365";
                ArrowIcon = "\uEDDB";
            }
        }
    }
}
