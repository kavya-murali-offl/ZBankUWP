using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;

namespace ZBank.AppEvents
{
    public class ViewNotifier
    {
        public event Action<FrameContentChangedArgs> FrameContentChanged;
        public void OnFrameContentChanged(FrameContentChangedArgs args)
        {
            FrameContentChanged?.Invoke(args);
        }


        public event Action<DashboardDataUpdatedArgs> DashboardDataChanged;
        public void OnDashboardDataChanged(DashboardDataUpdatedArgs args)
        {
            DashboardDataChanged?.Invoke(args);
        }


        public Action AccountInserted;
        public void OnAccountInserted()
        {
            AccountInserted?.Invoke();
        }

        public event Action<bool> LoggedIn;
        public void OnSuccessfulLogin(bool IsLoginSuccess)
        {
            LoggedIn?.Invoke(IsLoginSuccess);
        }

        public event Action<AccountsListUpdatedArgs> AccountsListUpdated;
        public void OnAccountsListUpdated(AccountsListUpdatedArgs args)
        {
            AccountsListUpdated?.Invoke(args);
        }

        public event Action<ElementTheme> ThemeChanged;
        public void OnThemeChanged(ElementTheme theme)
        {
            ThemeChanged?.Invoke(theme);
        }

        private ViewNotifier() { }

        private static ViewNotifier instance = null;

        public static ViewNotifier Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ViewNotifier();
                }
                return instance;
            }
        }
    }
}
