using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.BusinessObjects;

namespace ZBank.Entities
{
    public class CurrentAccount : Account
    {

        public decimal CHARGES = 100;

        public decimal InterestRate { get; set; }
    }
}
