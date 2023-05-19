using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;

namespace ZBank.Entity.BusinessObjects
{
    public class DashboardDataModel
    {
        private static DashboardDataModel _instance;
        private DashboardDataModel() { }

        public static DashboardDataModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DashboardDataModel();
                }
                return _instance;
            }
        }

        public DashboardCardModel BalanceCard { get; set; }

        public DashboardCardModel BeneficiariesCard { get; set; }

        public DashboardCardModel DepositCard { get; set; }

        public DashboardCardModel IncomeExpenseCard { get; set; }

        public ObservableCollection<CardBObj> AllCards { get; set; }

        public ObservableCollection<Beneficiary> AllBeneficiaries { get; set; }

        public ObservableCollection<TransactionBObj> LatestTransactions { get; set; }
    }

    public class DashboardCardModel
    {
        public string PrimaryKey { get; set; }
        public object PrimaryValue { get; set; }
        public string SecondaryKey1 { get; set; }
        public object SecondaryValue1 { get; set; }
        public string SecondaryKey2 { get; set; }
        public object SecondaryValue2 { get; set; }
    }
}

