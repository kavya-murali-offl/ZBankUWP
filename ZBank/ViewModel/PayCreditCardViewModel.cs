using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.View.Modals;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBank.Services;
using ZBankManagement.Entities.BusinessObjects;
using ZBank.ViewModel.VMObjects;

namespace ZBank.ViewModel
{
    internal class PayCreditCardViewModel : ViewModelBase
    {

        public CreditCard Card;

        public PayCreditCardViewModel(IView view, CreditCard card)
        {
            View = view;
            Card = card;
            Reset();
            ViewNotifier.Instance.AccountsListUpdated += OnAccountsListUpdated;
        }

        public void OnLoaded()
        {
            ViewNotifier.Instance.AccountsListUpdated += OnAccountsListUpdated;
            ViewNotifier.Instance.CreditCardSettled += OnCreditCardSettled;
            LoadAllAccounts();
            Reset();
        }

        private async void OnCreditCardSettled(bool isSettled, string message)
        {
            if (isSettled)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = $"Credit Card Amount Settled Successfully",
                        Type = NotificationType.SUCCESS
                    });
                });
            }
            else
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }

        private ObservableCollection<AccountBObj> _accounts = new ObservableCollection<AccountBObj>();  

        public ObservableCollection<AccountBObj> Accounts
        {
            get { return _accounts; }
            set { Set(ref _accounts, value); }
        }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set { Set(ref _amount, value); }
        }


        private decimal _availableBalance;

        public decimal AvailableBalance
        {
            get { return _availableBalance; }
            set { Set(ref _availableBalance, value); }
        }


        private AccountBObj _selectedAccount = null;

        public AccountBObj SelectedAccount
        {
            get { return _selectedAccount; }
            set { Set(ref _selectedAccount, value); }
        }

        private void OnAccountsListUpdated(AccountsListUpdatedArgs args)
        {
            Accounts = new ObservableCollection<AccountBObj>(args.AccountsList);
            SelectedAccount = Accounts.FirstOrDefault();
            AvailableBalance = SelectedAccount.Balance;
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                IsTransactionAccounts = true,
                AccountType = null,
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void OnUnloaded()
        {
            ViewNotifier.Instance.AccountsListUpdated -= OnAccountsListUpdated;
        }

        internal void Reset()
        {
            FieldErrors["Account"] = string.Empty;
            FieldErrors["Amount"] = string.Empty;
            SelectedAccount = null;
            Amount = 0;
            Accounts = Accounts;
        }

        internal void PayCard()
        {
            if (ValidateFields() && CheckBalance())
            {
                MakePayment();
            }
        }

        private bool CheckBalance()
        {
            if (AvailableBalance < Amount)
            {
                FieldErrors["Amount"] = $"Insufficient Balance. Amount should be less than Rs. {AvailableBalance:C}";
                return false;
            }
            return true;
        }

        private ObservableDictionary<string, string> _fieldErrors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, string> FieldErrors
        {
            get => _fieldErrors;
            set => Set(ref _fieldErrors, value);    
        }

        private bool ValidateFields()
        {
            var list = new List<string>()
            {
                "AccountNumber"
            };

            ValidateObject(FieldErrors, typeof(AccountBObj), list, SelectedAccount ?? new AccountBObj());
            FieldErrors["Account"] = FieldErrors["AccountNumber"]?.Replace("AccountNumber", "Account");

            ValidateField(FieldErrors, "Amount", Amount);
           
            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;

            return true;
        }

        private void MakePayment()
        {
            PayCreditCardRequest request = new PayCreditCardRequest()
            {
                CreditCard = Card,
                CustomerID = Repository.Current.CurrentUserID,
                PaymentAccount = SelectedAccount,
                PaymentAmount = Amount,
            };

            IPresenterCallback<PayCreditCardResponse> presenterCallback = new PayCreditCardPresenterCallback(this);
            UseCaseBase<PayCreditCardResponse> useCase = new PayCreditCardUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class PayCreditCardPresenterCallback : IPresenterCallback<PayCreditCardResponse>
        {
            private PayCreditCardViewModel ViewModel { get; set; }

            public PayCreditCardPresenterCallback(PayCreditCardViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(PayCreditCardResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCreditCardSettled(true, "Credit Card Payment Successful");
                    ViewNotifier.Instance.OnCloseDialog();
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                    ViewNotifier.Instance.OnCreditCardSettled(false, response.Message);
                });
               

            }
        }


        private class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private PayCreditCardViewModel ViewModel { get; set; }

            public GetAllAccountsPresenterCallback(PayCreditCardViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                    {
                        AccountsList = response.Accounts
                    };
                    ViewNotifier.Instance.OnAccountsListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = response.Message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }
    }
}
