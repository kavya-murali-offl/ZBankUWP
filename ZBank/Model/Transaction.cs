using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Model
{
    public class Transaction
    {

        public Transaction(TransactionType transactionType, ModeOfPayment mode, decimal amount, DateTime recordedOn, decimal balance, string otherAccount, string description) { 
            TransactionType = transactionType;  
            ModeOfPayment = mode;
            Amount = amount;
            RecordedOn = recordedOn;
            Balance = balance;
            OtherAccount = otherAccount;    
            Description = description;
        }    
        public string ID { get; set; }

        public TransactionType TransactionType { get; set; }

        public ModeOfPayment ModeOfPayment { get; set; }

        public decimal Amount { get; set; }

        public DateTime RecordedOn { get; set; }

        public decimal Balance { get; set; }

        public string OwnerAccount { get; set; }

        public string OtherAccount { get; set; }

        public string Description { get; set; }

        public string CardNumber { get; set; }

    }
    public enum TransactionType
    {
        EXPENSE, INCOME
    }

    public enum ModeOfPayment
    {
        CREDIT_CARD, DEBIT_CARD, DIRECT
    }
}
