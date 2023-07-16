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
    }

    [Table("TransactionMetaData")]
    public class TransactionMetaData
    {
        [PrimaryKey]
        public string ID { get; set; }  
        
        public string ReferenceID { get; set; }

        public string AccountNumber { get; set; }

        public decimal ClosingBalance { get; set; }
    }

    public enum TransactionType
    {
        SELF_TRANSFER, TRANSFER, CARD_PAYMENT
    }

}
