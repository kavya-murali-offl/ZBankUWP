﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entities
{
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public decimal MinimumBalance { get; set; }

    }
}
