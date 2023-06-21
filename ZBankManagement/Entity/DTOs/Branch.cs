using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities
{
    [Table("Branch")]
    public class Branch
    {
        [PrimaryKey]
        public string BranchID { get; set; } 
        
        public string BranchName { get; set; }

        public string BankID { get; set; }

        public string IfscCode { get; set; }

        public override string ToString() => BranchName + " - " + IfscCode;
    }
}
