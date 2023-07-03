using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entities
{
    [Table("Card")]
    public class Card
    {
        [PrimaryKey]
        public string CardNumber { get; set; }

        public DateTime LinkedOn { get; set; }

        public string Pin { get; set; }

        public CardType Type { get; set; }

        public string CustomerID { get; set; }

        public string CVV { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public decimal TransactionLimit { get; set; }

    }

    public enum CardType
    {
        CREDIT=1, DEBIT=0
    }
}
