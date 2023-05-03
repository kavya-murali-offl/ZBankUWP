using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities
{
    public class Beneficiary
    {
        public Beneficiary(string accountNumber, string name, string ifscCode) { 
            AccountNumber = accountNumber;
            Name = name;
            IFSCCode = ifscCode;
        }

        public string AccountNumber { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string IFSCCode { get; set; }
    }
}
