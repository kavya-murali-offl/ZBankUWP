using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZBank.Entities.BusinessObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.AppEvents.AppEventArgs;
using ZBank.View;
using ZBank.AppEvents;
using Windows.UI.Core;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Data;
using ZBank.ViewModel.VMObjects;
using Windows.UI.Popups;
using ZBank.Entities;
using Windows.ApplicationModel.Core;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.View.UserControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ZBank.Services;

namespace ZBank.ViewModel
{
    public class TransactionViewModel : ViewModelBase
    {
        public IView View;

        public ICommand PreviousCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        public IList<int> DefinedRows { get; private set; }

        public IEnumerable<TransactionType> TransactionTypes { get => Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>(); }

        public TransactionViewModel(IView view)
        {
            View = view;
            DefinedRows = new List<int>()
            {
              1, 5, 10, 25, 50
            };
            NextCommand = new RelayCommand(GoToNextPage, IsNextButtonEnabled);
            PreviousCommand = new RelayCommand(GoToPreviousPage, IsPreviousButtonEnabled);
            RowsPerPage = DefinedRows.FirstOrDefault();
        }

        public void OnPageLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated += UpdateTransactionsData;
            ViewNotifier.Instance.CancelPaymentRequested += NewTransactionAdded;
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.RightPaneContentUpdated += PaneClosed;
            CurrentPageIndex = 0;
            RowsPerPage = DefinedRows.First();
            LoadAllAccounts();
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                IsTransactionAccounts = true,
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            AccountsList = new ObservableCollection<AccountBObj>(args.AccountsList);
            if (AccountsList.Count > 0)
            {
                UpdateSelectedAccount(AccountsList.ElementAt(0));
            }
        }

        internal void UpdateSelectedAccount(AccountBObj accountBObj)
        {
            SelectedAccount = accountBObj;
            CurrentPageIndex = 0;
            RowsPerPage = DefinedRows.FirstOrDefault();
            LoadAllTransactionsData();
        }

        public void LoadAllTransactionsData()
        {
            if(SelectedAccount != null)
            {
                GetAllTransactionsRequest request = new GetAllTransactionsRequest()
                {
                    CustomerID = Repository.Current.CurrentUserID,
                    CurrentPageIndex = CurrentPageIndex,
                    RowsPerPage = RowsPerPage,
                    AccountNumber = SelectedAccount.AccountNumber
                };

                IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsPresenterCallback(this);
                UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
                useCase.Execute();
            }
           
        }

        private void UpdateTransactionsData(TransactionPageDataUpdatedArgs args)
        {
            InViewTransactions = new ObservableCollection<TransactionBObj>(args.TransactionList);
            TotalPages = args.TotalPages;
            (NextCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        private void NewTransactionAdded(bool isPaymentCompleted)
        {
            if (isPaymentCompleted)
            {
                CurrentPageIndex = 0;
                RowsPerPage = DefinedRows.FirstOrDefault();
                LoadAllTransactionsData();
            }
        }

        private void PaneClosed(FrameworkElement obj)
        {
            if (obj == null)
            {
                InViewTransaction = null;
            }
        }
      
        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactionsData;
            ViewNotifier.Instance.CancelPaymentRequested -= NewTransactionAdded;
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.RightPaneContentUpdated -= PaneClosed;
        }

       

        private bool IsPreviousButtonEnabled()
        {
            return CurrentPageIndex > 0;
        }

        private bool IsNextButtonEnabled()
        {
            return CurrentPageIndex + 1 < TotalPages;
        }

        private void GoToPreviousPage(object parameter)
        {
            CurrentPageIndex--;
            LoadAllTransactionsData();
        }

        private void GoToNextPage(object parameter)
        {
            CurrentPageIndex++;
            LoadAllTransactionsData();
        }


        internal void UpdateView(TransactionBObj transaction)
        {
            InViewTransaction = transaction;
            ViewTransaction viewTransaction = new ViewTransaction();
            viewTransaction.InViewTransaction = transaction;
            ViewNotifier.Instance.OnRightPaneContentUpdated(viewTransaction);
        }

        internal void UpdateRows(int rows)
        {
            CurrentPageIndex = 0;
            RowsPerPage = rows;
            LoadAllTransactionsData();
        }

        private int _currentPageIndex = 0;

        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set => Set(ref _currentPageIndex, value);
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => Set(ref _totalPages, value);
        }


        private int _rowsPerPage = 0;

        public int RowsPerPage
        {
            get => _rowsPerPage;
            set => Set(ref _rowsPerPage, value);
        }

        private ObservableCollection<TransactionBObj> _inViewTransactions = new ObservableCollection<TransactionBObj>();

        public ObservableCollection<TransactionBObj> InViewTransactions
        {
            get => _inViewTransactions;
            set => Set(ref _inViewTransactions, value);
        }

        private TransactionBObj _inViewTransaction = null;

        public TransactionBObj InViewTransaction
        {
            get => _inViewTransaction;
            set => Set(ref _inViewTransaction, value);
        }

        private ObservableCollection<AccountBObj> _accountsList = new ObservableCollection<AccountBObj>();

        public ObservableCollection<AccountBObj> AccountsList
        {
            get { return _accountsList; }
            set { Set(ref _accountsList, value); }
        }

        private AccountBObj _selectedAccount = null;

        public AccountBObj SelectedAccount
        {
            get => _selectedAccount;
            set => Set(ref _selectedAccount, value);
        }


        private class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private TransactionViewModel ViewModel { get; set; }

            public GetAllAccountsPresenterCallback(TransactionViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                    {
                        AccountsList = new ObservableCollection<AccountBObj>(response.Accounts)
                    };
                    ViewNotifier.Instance.OnAccountsListUpdated(args);

                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Duration = 3000,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

        private class GetAllTransactionsPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
        {
            public TransactionViewModel ViewModel { get; set; }

            public GetAllTransactionsPresenterCallback(TransactionViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllTransactionsResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                    {
                        TransactionList = response.Transactions,
                        TotalPages = response.TotalPages
                    };

                    ViewNotifier.Instance.OnTransactionsListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {

            }
        }

        private class GetAllBeneficiariesInTransactionsPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
        {
            public TransactionViewModel ViewModel { get; set; }

            public GetAllBeneficiariesInTransactionsPresenterCallback(TransactionViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllBeneficiariesResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    BeneficiaryListUpdatedArgs args = new BeneficiaryListUpdatedArgs()
                    {
                        BeneficiaryList = response.Beneficiaries
                    };
                    ViewNotifier.Instance.OnBeneficiaryListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
            }
        }
    }
}

//ResetFilterValues();
//FilterConditions["FromAccount"] = item => item.SenderAccountNumber == FilterValues["FromAccount"].ToString();
//FilterConditions["ToAccount"] = item => item.RecipientAccountNumber == FilterValues["ToAccount"].ToString();
////FilterConditions["FromDate"] = item => item.RecordedOn > FilterValues["FromDate"];
////FilterConditions["ToDate"] = item => item.RecordedOn < FilterValues["ToDate"];
        //private IDictionary<string, Func<TransactionBObj, bool>> FilterConditions = new Dictionary<string, Func<TransactionBObj, bool>>();
///
