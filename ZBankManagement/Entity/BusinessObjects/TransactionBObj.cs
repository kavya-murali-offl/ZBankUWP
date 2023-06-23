using System;
using Windows.UI.Xaml.Media;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj : Transaction
    {
        public string Name { get; set; }

        public string ArrowIcon { get; set; }

        public string PlusOrMinus { get; set; }

        public string BorderColor { get;  set; }

        public string BackgroundColor { get;  set; }

        public void SetDefault(TransactionType type)
        {
            //if (type == TransactionType.DEBIT)
            //{
            //    BorderColor = "#be3232";
            //    BackgroundColor = "#f5e1dd";
            //    ArrowIcon = "\uEDDC";
            //    PlusOrMinus = "-";
            //}
            //else if (type == TransactionType.CREDIT)
            //{
            //    BackgroundColor = "#eafde8";
            //    BorderColor = "#058365";
            //    ArrowIcon = "\uEDDB";
            //    PlusOrMinus = "+";
            //}
        }
    }
}
