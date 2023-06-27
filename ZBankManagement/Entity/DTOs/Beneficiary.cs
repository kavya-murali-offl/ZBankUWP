using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.Entities
{
    [Table("Beneficiary")]
    public class Beneficiary
    {
        [PrimaryKey]
        public string ID { get; set; }

        public string UserID { get; set; }

        public string AccountNumber { get; set; }
  
        public bool IsFavourite { get; set; }

        public string BeneficiaryName { get; set; }

        public BeneficiaryType BeneficiaryType { get; set; }
    }

    [Table("ExternalAccounts")]
    public class ExternalAccount
    {

        [PrimaryKey]
        public string ExternalAccountNumber { get; set; }

        public string ExternalIFSCCode { get; set; }

        public string ExternalName { get; set; }

        public string ProfilePicture { get; set; }

    }
}
