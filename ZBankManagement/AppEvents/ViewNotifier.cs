using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.AppEvents.AppEventArgs;

namespace ZBank.AppEvents
{
    public class ViewNotifier
    {

        public event Action LoadApp;
        public void OnLoadApp()
        {
            LoadApp?.Invoke();
        }

        public event Action<bool, string> CreditCardSettled;
        public void OnCreditCardSettled(bool IsSettled, string message)
        {
            CreditCardSettled?.Invoke(IsSettled, message);
        }

        public event Action PaymentResetRequested;
        public void OnPaymentResetRequested()
        {
            PaymentResetRequested?.Invoke();
        }

        public event Action<bool, Card> CardInserted;
        public void OnCardInserted(bool updated, Card updatedCard)
        {
            CardInserted?.Invoke(updated, updatedCard);
        }

        public event Action<bool, Card> LimitUpdated;
        public void OnUpdatedLimit(bool updated, Card updatedCard)
        {
            LimitUpdated?.Invoke(updated, updatedCard);
        }

        public event Action<bool, AccountBObj> AccountUpdated;
        public void OnAccountUpdated(bool updated, AccountBObj account=null)
        {
            AccountUpdated?.Invoke(updated, account);
        }

        public event Action<TermDepositAccount> DepositClosed;
        public void OnDepositClosed(TermDepositAccount account)
        {
            DepositClosed?.Invoke(account);
        }

        public event Action<Color> AccentColorChanged;
        public void OnAccentColorChanged(Color color)
        {
            AccentColorChanged?.Invoke(color);
        }

        public event Action<Customer> GetCustomerSuccess;
        public void OnGetCustomerSuccess(Customer customer)
        {
            GetCustomerSuccess?.Invoke(customer);
        }


        public event Action<FrameworkElement> RightPaneContentUpdated;
        public void OnRightPaneContentUpdated(FrameworkElement frameworkElement)
        {
            RightPaneContentUpdated?.Invoke(frameworkElement);
        }

        public bool PaymentInProgress { get; set; }

        public event Action<string> LoginError;
        public void OnLoginError(string error)
        {
            LoginError?.Invoke(error);
        }

        public event Action<string> CurrentUserChanged;
        public void OnCurrentUserChanged(string id)
        {
            CurrentUserChanged?.Invoke(id);
        }

        public event Action<bool> RequestFailed;
        public void OnRequestFailed(bool close)
        {
            RequestFailed?.Invoke(close);
        }

        public event Action CloseDialog;
        public void OnCloseDialog()
        {
            CloseDialog?.Invoke();
        }

        public event Action<Beneficiary> BeneficiaryRemoved;
        public void OnBeneficiaryRemoved(Beneficiary beneficiary)
        {
            BeneficiaryRemoved?.Invoke(beneficiary);
        }


        public event Action<Beneficiary, bool> BeneficiaryAddOrUpdated;
        public void OnBeneficiaryAddOrUpdated(Beneficiary beneficiary, bool isAdded)
        {
            BeneficiaryAddOrUpdated?.Invoke(beneficiary, isAdded);
        }

        public event Action<int> CurrentPaymentStepChanged;
        public void OnCurrentPaymentStepChanged(int step)
        {
            CurrentPaymentStepChanged?.Invoke(step);
        }

        public event Action<bool> CancelPaymentRequested;
        public void OnCancelPaymentRequested(bool isPaymentCompleted)
        {
            CancelPaymentRequested?.Invoke(isPaymentCompleted);
        }

        public event Action<bool> AccountInserted;
        public void OnAccountInserted (bool isSuccess)
        {
            AccountInserted?.Invoke(isSuccess);
        }

        public event Action<Notification> NotificationStackUpdated;
        public void OnNotificationStackUpdated(Notification notification)
        {
            NotificationStackUpdated?.Invoke(notification);
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

        public event Action<ShellContentChangedArgs> ShellContentChanged;
        public void OnShellContentChanged(ShellContentChangedArgs args)
        {
            ShellContentChanged?.Invoke(args);
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

        public event Action<Customer> SignupSuccess;

        public void OnSignupSuccess(Customer insertedCustomer)
        {
            SignupSuccess?.Invoke(insertedCustomer);
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
