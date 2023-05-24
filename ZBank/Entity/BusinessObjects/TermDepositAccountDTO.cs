using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    [Table("TermDepositAccount")]
    public class TermDepositAccountDTO
    {
        [PrimaryKey] 
        public string AccountNumber { get ; set; }  


    }
}
