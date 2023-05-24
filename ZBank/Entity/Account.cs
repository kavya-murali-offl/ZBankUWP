using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.Entity.BusinessObjects
{
    public class Account : AccountDTO
    {

        public Branch BranchID { get; set; }

        public Branch BankID { get; set; }

        public Branch BranchName { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }

        public override string ToString() => AccountNumber + " - " + AccountType.ToString();
    }
}