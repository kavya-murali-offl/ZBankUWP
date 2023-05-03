using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class DebitCardDTO : DebitCard
    {

        public DebitCardDTO(string cardNumber, CardType type) : base(cardNumber, type)
        {

        }

        public DebitCardDTO() { }
    }
}
