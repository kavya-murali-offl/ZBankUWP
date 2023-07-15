using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZBank.Entities;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBankManagement.Entities.BusinessObjects
{
    public class BeneficiaryBObj : ICloneable
    {
        public string ID { get; set; }

        public string UserID { get; set; }

        public string AccountNumber { get; set; }

        public bool IsFavourite { get; set; }

        public string BeneficiaryName { get; set; }

        public BeneficiaryType BeneficiaryType { get; set; }

        public string IFSCCode { get; set; }

        public string ExternalIFSCCode { get; set; }

        public string RequiredIFSCCode { get => BeneficiaryType == BeneficiaryType.WITHIN_BANK ? IFSCCode : ExternalIFSCCode; }    

        public override string ToString()
        {
            return AccountNumber + " - " + BeneficiaryName;
        }

        public object Clone()
        {
            return new BeneficiaryBObj()
            {
                ID = ID,
                AccountNumber = AccountNumber,
                UserID = UserID,
                BeneficiaryName = BeneficiaryName,
                IFSCCode = IFSCCode,
                ExternalIFSCCode = ExternalIFSCCode,
                BeneficiaryType = BeneficiaryType,
                IsFavourite = IsFavourite,
            };
        }
    }
}
