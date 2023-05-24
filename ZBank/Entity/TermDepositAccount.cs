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

    public class TermDepositAccount : Account
    {

        //public TermDepositAccount(decimal amount, int tenureInMonths, string fromAccountNumber) : base(amount)
        //{
        //    TenureInMonths = tenureInMonths;
        //    InterestRate = GetFDInterestRate(tenureInMonths);
        //    MaturityAmount = MaturityAmountCalculator(amount, tenureInMonths);
        //    MaturityDate = MaturityDateCalculator(DateTime.Now, tenureInMonths);
        //    FromAccountNumber = fromAccountNumber;
        //    RepaymentAccountNumber = "11111";
        //}
    
        public decimal InterestRate { get; private set; }

        public decimal MaturityAmount { get; set; }

        public int TenureInMonths { get; set; }

        public string FromAccountNumber { get; set; }   

        public string RepaymentAccountNumber { get; set; }

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

        public decimal GetFDInterestRate(int tenureInMonths)
        {
            switch (tenureInMonths)
            {
                case 3: return 9m;
                case 6: return 10m;
                case 9: return 11m;
                case 12: return 12m;
                case 24: return 12.50m;
                case 48: return 14m;
                default: return 0;
            }
        }
    }
}
