using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity;

namespace ZBank.Entities.BusinessObjects
{
    public class AccountBObj : Account
    {

        public string CardNumber { get; set; }

        public string BranchName { get; set; }

        public string BankID { get; set; }

        public string IfscCode { get; set; }

        public Card LinkedCard { get; set; }

        public IEnumerable<TransactionBObj> Transactions { get; set; }

        public override string ToString() => AccountNumber + " - " + AccountType.ToString();
    }
}