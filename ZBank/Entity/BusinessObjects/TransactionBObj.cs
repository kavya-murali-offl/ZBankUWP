using System;
using Windows.UI.Xaml.Media;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj : Transaction
    {
        public Beneficiary OtherAccount { get; set; }

        public string ArrowIcon { get; set; }

        public string BorderColor { get;  set; }

        public string BackgroundColor { get;  set; }

    }
}
