using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace BankManagementDB.Events
{

    public class AppEvents
    {
        public event Action<bool> IsLoggedIn;
        public event Action<IEnumerable<Account>> IsAccountsLoaded;

        public void OnSuccessfulLogin(bool IsLoginSuccess){
            IsLoggedIn?.Invoke(IsLoginSuccess);
        }

        public void OnGetAccountsSuccess(IEnumerable<Account> accounts)
        {
            IsAccountsLoaded?.Invoke(accounts);
        }
    }
}
