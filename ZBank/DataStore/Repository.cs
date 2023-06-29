using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using ZBank.Services;

namespace ZBank.DataStore
{
    internal class Repository
    {

        public string CurrentUserID { get; set; }

        static Repository()
        {
            Current = new Repository();
        }

        static public Repository Current { get; }

    }

}
