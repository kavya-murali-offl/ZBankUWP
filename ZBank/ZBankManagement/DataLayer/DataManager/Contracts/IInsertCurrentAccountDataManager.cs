using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementDB.Data.DataManager
{
    public interface IInsertCurrentAccountDataManager
    {
        bool InsertCurrentAccount(CurrentAccountDTO currentAccount);
    }
}
