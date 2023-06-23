using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entities
{
    [Table("Transactions")]
    public class Transaction
    {
        [PrimaryKey]
        public string ReferenceID { get; set; }

        public decimal Amount { get; set; }

        public DateTime RecordedOn { get; set; }

        public TransactionType TransactionType { get; set; }

        public string SenderAccountNumber { get; set; }

        public string RecipientAccountNumber { get; set; }

        public string Description { get; set; }

        public decimal SenderBalance { get; set; }

        public decimal RecipientBalance { get; set; }
    }

    //[Table("TransactionSenders")]
    //public class TransactionSender
    //{
    //    [PrimaryKey]
    //    public string ReferenceID { get; set; }

    //    public decimal SenderBalance { get; set; }

    //    public string SenderAccountNumber { get; set; }
    //}

    //[Table("TransactionRecipients")]
    //public class TransactionRecipient
    //{
    //    [PrimaryKey]
    //    public string ReferenceID { get; set; }

    //    public string RecipientAccountNumber { get; set; }

    //    public decimal RecipientBalance { get; set; }
    //}

    public enum TransactionType
    {
        INTERNAL, EXTERNAL
    }

    public enum ModeOfPayment
    {
        NONE, CREDIT_CARD, DEBIT_CARD, DIRECT
    }
}
