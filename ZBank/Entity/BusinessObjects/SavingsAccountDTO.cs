﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    [Table("SavingsAccount")]
    public class SavingsAccountDTO
    {

        [PrimaryKey]
        public string ID { get; set; }

    }
}
