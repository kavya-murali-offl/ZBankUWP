using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsResponse
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
}
