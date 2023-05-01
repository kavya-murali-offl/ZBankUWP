using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Model;

namespace ZBank.Entity
{
    public class CreditCard 
    {

        public CreditCardType CreditCardType { get; set; }

        public decimal TotalOutstanding { get; set; }

        public decimal MinimumOutstanding { get; set; }

        public decimal Interest { get; set; }

        public decimal CreditLimit { get; set; }

    }

    public enum CreditCardType
    {
        VISA=0,
        MASTERCARD=1,
        RUPAY=2
    }

}
