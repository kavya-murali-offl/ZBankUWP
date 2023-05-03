using System;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj
    {
        public TransactionBObj(TransactionType transactionType, ModeOfPayment mode, decimal amount, DateTime recordedOn, decimal balance, string otherAccount, string description)
        {
            TransactionType = transactionType;
            ModeOfPayment = mode;
            Amount = "$"+ amount.ToString();
            RecordedOn = recordedOn;
            Balance = "$" + balance.ToString();
            OtherAccount = otherAccount;
            Description = description;
            if(TransactionType == TransactionType.EXPENSE) {
                BorderColor = "#be3232";
                BackgroundColor = "#f5e1dd";
                ArrowIcon = "\uEDDC";
            }
            else if(TransactionType == TransactionType.INCOME) {
                BackgroundColor = "#eafde8";
                BorderColor = "#058365";
                ArrowIcon = "\uEDDB";
            }
        }
        public string ID { get; set; }

        public TransactionType TransactionType { get; set; }

        public ModeOfPayment ModeOfPayment { get; set; }

        public string Amount { get; set; }

        public DateTime RecordedOn { get; set; }

        public string Balance { get; set; }

        public string OwnerAccount { get; set; }

        public string OtherAccount { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }

        public string ArrowIcon { get; set; }

        public string BorderColor { get; private set; }

        public string BackgroundColor { get; private set; }
    }
}
