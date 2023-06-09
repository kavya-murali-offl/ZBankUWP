﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;

namespace ZBank.AppEvents.AppEventArgs
{
    public class DashboardDataUpdatedArgs
    {

        public DashboardCardModel BalanceCard { get; set; }

        public DashboardCardModel BeneficiariesCard { get; set; }

        public DashboardCardModel DepositCard { get; set; }

        public DashboardCardModel IncomeExpenseCard { get; set; }

        public IEnumerable<CardBObj> AllCards { get; set; }

        public IEnumerable<Beneficiary> AllBeneficiaries { get; set; }

        public IEnumerable<TransactionBObj> LatestTransactions { get; set; }

        public IEnumerable<AccountBObj> AllAccounts { get; set; }
    }
}
