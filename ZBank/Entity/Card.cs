﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ZBank.Entities
{
    public class Card
    {
        public Card() { }
        public Card(string cardNumber, CardType cardType) {
            CardNumber = cardNumber;
            Type = cardType;
            CreatedOn = DateTime.Now;
            Pin = "1111";
            ExpiryMonth = "12";
            ExpiryYear = "1992";
        }

        public string ID { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Pin { get; set; }

        public CardType Type { get; set; }

        public string CardNumber { get; set; }

        public string CustomerID { get; set; }

        public string CVV { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }
    }

    public enum CardType
    {
        CREDIT=1, DEBIT=0
    }
}
