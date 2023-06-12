using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBankManagement.AppEvents.AppEventArgs;

namespace ZBank.AppEvents
{
    public class ViewNotifier
    {

        public event Action<bool> OnSuccess;
        public void OnSuccessfulEvent (bool isSuccess)
        {
            OnSuccess?.Invoke(isSuccess);
        }

        public event Action<NotificationOpeningEventArgs> NotificationOpening;
        public void ONNotificationOpening(NotificationOpeningEventArgs args)
        {
            NotificationOpening?.Invoke(args);
        }

        public event Action<NotifyUserArgs> NotificationStackUpdated;
        public void OnNotificationStackUpdated(NotifyUserArgs args)
        {
            NotificationStackUpdated?.Invoke(args);
        }

        public event Action<BranchListUpdatedArgs> BranchListUpdated;
        public void OnBranchListUpdated(BranchListUpdatedArgs args)
        {
            BranchListUpdated?.Invoke(args);
        }

        public event Action<BeneficiaryListUpdatedArgs> BeneficiaryListUpdated;
        public void OnBeneficiaryListUpdated(BeneficiaryListUpdatedArgs args)
        {
            BeneficiaryListUpdated?.Invoke(args);
        }

        public event Action<CardDataUpdatedArgs> CardsDataUpdated;
        public void OnCardsPageDataUpdated(CardDataUpdatedArgs args)
        {
            CardsDataUpdated?.Invoke(args);
        }

        public event Action<TransactionPageDataUpdatedArgs> TransactionListUpdated;
        public void OnTransactionsListUpdated(TransactionPageDataUpdatedArgs args)
        {
            TransactionListUpdated?.Invoke(args);
        }

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
