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

        public TransactionType TransactionType { get; set; }

        public ModeOfPayment ModeOfPayment { get; set; }

        public decimal Amount { get; set; }

        public DateTime RecordedOn { get; set; }

        public decimal Balance { get; set; }

        public string OwnerAccountNumber { get; set; }

        public string OtherAccountNumber { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }
    }

    public enum TransactionType
    {
        EXPENSE, INCOME
    }

    public enum ModeOfPayment
    {
        NONE, CREDIT_CARD, DEBIT_CARD, DIRECT
    }
}
