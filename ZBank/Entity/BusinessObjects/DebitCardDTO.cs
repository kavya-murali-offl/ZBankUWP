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
        public string ID { get; set; }

        public string AccountID { get; set; }
    }
}
