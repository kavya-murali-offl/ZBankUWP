using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class CardBObj
    {
        public CardBObj(string cardNumber, CardType type, string background)
        {
            CardNumber = cardNumber.Trim();
            CreatedOn = DateTime.Now;
            Pin = "1111";
            ExpiryMonth = "12";
            ExpiryYear = "1992";
            Type = type;
            BackgroundImage = background;
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

        public string BackgroundImage { get; set; }
    }
}
