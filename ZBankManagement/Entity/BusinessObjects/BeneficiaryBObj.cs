using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZBank.Entities;

namespace ZBankManagement.Entity.BusinessObjects
{
    public class BeneficiaryBObj : Beneficiary
    {
        public string IFSCCode { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return AccountNumber + " - " + Name;
        }
    }
}
