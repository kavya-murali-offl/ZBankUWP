﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    [Table("CurrentAccount")]
    public class CurrentAccountDTO 
    {
        [PrimaryKey] 
        public string AccountNumber { get; set; }  
        
        public decimal InterestRate { get; set; }
        public decimal MinimumBalance { get; set; }

    }
}
