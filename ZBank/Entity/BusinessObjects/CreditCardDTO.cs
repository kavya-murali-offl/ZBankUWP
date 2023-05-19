using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ZBank.Entities.BusinessObjects
{
    [Table("CreditCard")]
    public class CreditCardDTO
    {

        [PrimaryKey]
        public string CardNumber { get; set; }

        public CreditCardProvider Provider { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal MinimumOutstanding { get; set; }

        public decimal Interest { get; set; }

        public decimal CreditLimit { get; set; }
    }
}
