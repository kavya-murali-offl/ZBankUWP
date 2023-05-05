using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.EnumerationType;
using ZBank.ZBankManagement.UseCase.GetAllAccounts;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.Interface
{
    public interface IGetAccountDataManager
    {
        void GetAllAccounts(string id);

        void GetAllAccounts(GetAllAccountsRequest request, GetAllAccountsCallback callback);
    }
}
