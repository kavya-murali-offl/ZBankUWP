using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.Entities
{
    public class DebitCard : Card
    {

        public DebitCard() { }  
        public DebitCard(string cardNumber, CardType type) : base(cardNumber, type)
        {

        }
        public string AccountID { get; set; }
    }
}
