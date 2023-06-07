using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entity
{
    [Table("Bank")]
    public class Bank
    {
        [PrimaryKey]
        public int ID { get; set; }

        public string Name { get; set; }

        public string TagLine { get; set; }

    }
}
