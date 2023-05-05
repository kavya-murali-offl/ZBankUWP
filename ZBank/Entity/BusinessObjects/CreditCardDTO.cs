using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ZBank.Entities.BusinessObjects
{
    [Table("CreditCard")]
    public class CreditCardDTO
    {

        [PrimaryKey]
        public string ID { get; set; }

        public CreditCardProvider Provider { get; set; }

    }
}
