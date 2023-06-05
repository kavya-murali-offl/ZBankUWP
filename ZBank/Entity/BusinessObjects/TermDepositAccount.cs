using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.EnumerationTypes;

namespace ZBank.Entities
{

    public class TermDepositAccount : AccountBObj
    {

        public void SetDefault()
        {
            InterestRate = GetFDInterestRate(TenureInMonths);
            MaturityAmount = MaturityAmountCalculator(Balance, TenureInMonths);
            MaturityDate = MaturityDateCalculator(DateTime.Now, TenureInMonths);
        }

        public decimal InterestRate { get; private set; }

        public decimal MaturityAmount { get; set; }

        public int TenureInMonths { get; set; }

        public string FromAccountNumber { get; set; }   

        public string RepaymentAccountNumber { get; set; }

        public decimal MinimumBalance { get; set; }

        public DepositType DepositType { get; set; }

        public DateTime? DepositStartDate { get; set; }

        public DepositType FDType { get; set; }

        public DateTime MaturityDate { get; set; }

        public decimal MaturityAmountCalculator(decimal amount, decimal interestRate)
        {
            return amount + (amount * (interestRate / 100));
        }

        public DateTime MaturityDateCalculator(DateTime date, int months)
        {
            return date.AddMonths(months);
        }

        public static decimal GetFDInterestRate(int tenureInMonths)
        {
            switch (tenureInMonths)
            {
                case 3: return 9m;
                case 6: return 10m;
                case 9: return 11m;
                case 12: return 12m;
                case 24: return 12.5m;
                case 48: return 14m;
                default: return 0;
            }
        }
    }
}
