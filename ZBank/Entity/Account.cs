using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities
{
    [Table("Account")]
    public class Account
    {
        public Account() { }

        public Account(decimal amount) {
            AccountNumber = "1111111111111111111";
            IFSCCode = "ZBNK1233";
            AccountName = "xxx";
            AccountStatus = AccountStatus.ACTIVE;
            OpenedOn = DateTime.Now;
            Currency = Currency.INR;
            Amount = amount;
        }

        [PrimaryKey]
        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }

        public string AccountName { get; set; }

        public string UserID { get; set; }

        public string CreatedOn { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public DateTime OpenedOn { get; set; }

        public Currency Currency { get; set; }

        public decimal Amount { get; set; }
    }
}
