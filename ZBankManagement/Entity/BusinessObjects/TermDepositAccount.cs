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
            InterestRate = GetFDInterestRate(Tenure);
            MaturityAmount = MaturityAmountCalculator(Balance, Tenure);
            MaturityDate = MaturityDateCalculator(DateTime.Now, Tenure);
        }

        public decimal InterestRate { get; set; }

        public decimal MaturityAmount { get; set; }

        public int Tenure { get; set; }

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
                case int n when (n <= 3): return 9m;
                case int n when (n <= 6): return 10m;
                case int n when (n <= 9): return 11m;
                case int n when (n <= 10): return 12m;
                case int n when (n <= 24): return 12.5m;
                case int n when (n <= 36): return 14m;
                default: return 0;
            }
        }

        internal decimal CalculateClosingAmount(DateTime now)
        {
            int? months = ((now.Year - DepositStartDate?.Year) * 12) + now.Month - DepositStartDate?.Month;
            if(months.HasValue)
            {
                decimal interestRate = GetFDInterestRate(months.Value);
                return MaturityAmountCalculator(Balance, interestRate);
            }
            return 0;
        }
    }
}
