using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.Entities;

namespace BankManagementDB.Events
{

    public class AppEvents
    {
        private AppEvents() { 
        
        }

        private static AppEvents instance = null;

        public static AppEvents Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppEvents();
                }
                return instance;
            }
        }

        public event Action<bool> LoggedIn;
        public event Action<IEnumerable<Account>> AccountsLoaded;
        public event Action<ElementTheme> ThemeChanged;

        public void OnSuccessfulLogin(bool IsLoginSuccess){
            LoggedIn?.Invoke(IsLoginSuccess);
        }

        public void OnGetAccountsSuccess(IEnumerable<Account> accounts)
        {
            AccountsLoaded?.Invoke(accounts);
        }

        public void OnThemeChanged(ElementTheme theme)
        {
            ThemeChanged?.Invoke(theme);
        }
    }
}
