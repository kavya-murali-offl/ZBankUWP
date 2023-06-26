using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZBank.Entities;

namespace ZBankManagement.Entities.BusinessObjects
{
    public class BeneficiaryBObj : Beneficiary
    {
        public string IFSCCode { get; set; }

        public string ExternalIFSCCode { get; set; }

        public string RequiredIFSCCode { get => BeneficiaryType == Entity.EnumerationTypes.BeneficiaryType.WITHIN_BANK ? IFSCCode : ExternalIFSCCode; }    

        public override string ToString()
        {
            return AccountNumber + " - " + BeneficiaryName;
        }
    }
}
