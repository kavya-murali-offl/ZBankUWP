using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Model
{
    public class Card
    {
        public Card(string cardNumber, CardType type) { 
            CardNumber = cardNumber;
            CreatedOn = DateTime.Now;
            Pin = "1111";
            ExpiryMonth = "12";
            ExpiryYear = "1992";
            Type = type;
        }

        public string ID { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Pin { get; set; }

        public string CardNumber { get; set; }

        public string AccountID { get; set; }

        public string CustomerID { get; set; }

        public string CVV { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public CardType Type { get; set; }
    }

    public enum CardType
    {
        CREDIT=1, DEBIT=0
    }
}
