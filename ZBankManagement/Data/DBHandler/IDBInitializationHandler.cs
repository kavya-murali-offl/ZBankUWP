using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.DataLayer.DBHandler
{
    interface IDBInitializationHandler
    {
        void CreateTables();
        void PopulateData();
    }
}
