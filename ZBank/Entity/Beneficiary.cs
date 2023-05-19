using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities
{
    [Table("Beneficiary")]
    public class Beneficiary
    {
        [PrimaryKey]
        public string ID { get; set; }

        public string UserID { get; set; }

        public string AccountNumber { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string IFSCCode { get; set; }

        // Ignore
        public override string ToString()
        {
            return AccountNumber + " - " + Name;
        }
    }
}
