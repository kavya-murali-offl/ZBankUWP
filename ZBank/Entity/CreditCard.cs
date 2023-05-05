using System;
using ZBank.Entities;

namespace ZBank.Entities
{
    public class CreditCard : Card
    {
        public CreditCard() { }

        public CreditCard(string cardNumber, CardType type) : base(cardNumber, type){
        
        }

        public CreditCardProvider CreditCardType { get; set; }

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
