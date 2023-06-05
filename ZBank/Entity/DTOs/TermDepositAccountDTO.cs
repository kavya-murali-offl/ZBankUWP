using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.EnumerationTypes;

namespace ZBank.Entities.BusinessObjects
{
    [Table("TermDepositAccount")]
    public class TermDepositAccountDTO
    {
        [PrimaryKey] 
        public string AccountNumber { get ; set; }  

        public int Tenure { get; set; }

        public decimal InterestRate { get; set; }

        public DepositType DepositType { get; set; }    

        public DateTime DepositStartDate { get; set; }

        public DateTime MaturityDate { get; set; }

        public decimal MaturityAmount { get; set; }

        public string RepaymentAccountNumber { get; set; }
    }
}
