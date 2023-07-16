using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entities
{
    public class SavingsAccount : AccountBObj
    {
        public decimal InterestRate { get; set; }

        public decimal MinimumBalance = 500m;
        public decimal TransactionLimit { get; set; }

    }
}
