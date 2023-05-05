using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.EnumerationType;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsRequest
    {
        public AccountType? AccountType { get; set; }

        public string userID { get; set; }
    }
}
