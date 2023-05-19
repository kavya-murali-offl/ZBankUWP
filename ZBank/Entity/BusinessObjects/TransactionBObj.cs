using System;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj : Transaction
    {
        public Beneficiary OtherAccount { get; set; }

        public string ArrowIcon { get; set; }

        public string BorderColor { get; private set; }

        public string BackgroundColor { get; private set; }

        public void SetBusinessObject()
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
