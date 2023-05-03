using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    public class CreditCardDTO : CreditCard
    {

        public CreditCardDTO(string cardNumber, CardType type) : base(cardNumber, type)
        {

        }

        public CreditCardDTO() { }
    }
}
