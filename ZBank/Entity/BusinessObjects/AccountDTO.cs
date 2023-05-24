﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.EnumerationType;
using ZBank.Entity;
using ZBank.Entity.EnumerationTypes;

namespace ZBank.Entities
{
    [Table("Account")]
    public class AccountDTO
    {
        [PrimaryKey]
        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }

        public string AccountName { get; set; }

        public string UserID { get; set; }

        public string CreatedOn { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public Currency Currency { get; set; }

        public decimal Amount { get; set; }

        public AccountType AccountType { get; set; }
    }
}
