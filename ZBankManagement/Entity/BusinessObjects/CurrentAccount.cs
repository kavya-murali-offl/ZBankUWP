﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entities
{
    public class CurrentAccount : AccountBObj
    {

        public decimal CHARGES = 100;

        public decimal InterestRate { get; set; }

        public decimal MinimumBalance = 500m;

        public decimal TransactionLimit = 20000000m;

    }
}
