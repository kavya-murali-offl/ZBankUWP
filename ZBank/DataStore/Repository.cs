using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.EnumerationTypes;
using ZBank.Services;

namespace ZBank.DataStore
{
    internal class Repository
    {
        public static string CurrentUserID { get; set; }

        public static void UpdateCurrentUserID(string id)
        {
            CurrentUserID = id;
        }

        static Repository()
        {
            Current = new Repository();
        }

        static public Repository Current { get; }
    }

}
