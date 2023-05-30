using System;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBankManagement.Utility;

namespace ZBank.Entities
{
    public class CreditCard : CardBObj
    {
        public CreditCardProvider CreditCardProvider { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal MinimumOutstanding { get; set; }

        public decimal Interest { get; set; }

        public decimal CreditLimit { get; set; }

    }

    public enum CreditCardProvider
    {
        VISA=0,
        MASTERCARD=1,
        RUPAY=2
    }

}
