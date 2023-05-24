using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity;

namespace ZBank.Entities.BusinessObjects
{
    public class Account : AccountDTO
    {
        public Branch Branch { get; set; }

        public DebitCard LinkedCard { get; set; }

        public IEnumerable<TransactionBObj> Transactions { get; set; }

        public override string ToString() => AccountNumber + " - " + AccountType.ToString();
    }
}