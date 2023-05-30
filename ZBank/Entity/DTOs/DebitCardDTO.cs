using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    [Table("DebitCard")]
    public class DebitCardDTO 
    {

        [PrimaryKey]
        public string CardNumber { get; set; }

        public string AccountNumber { get; set; }
    }
}
