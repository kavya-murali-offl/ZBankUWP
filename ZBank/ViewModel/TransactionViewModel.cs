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
               5, 10, 25, 50
            };
            NextCommand = new RelayCommand(GoToNextPage, IsNextButtonEnabled);
            PreviousCommand = new RelayCommand(GoToPreviousPage, IsPreviousButtonEnabled);
            RowsPerPage = DefinedRows.FirstOrDefault();
            FilterValues = new ObservableDictionary<string, object>();
            ResetFilterValues();
            SelectedAccount = AccountsList.FirstOrDefault();
            FilterConditions["FromAccount"] = item => item.SenderAccountNumber == FilterValues["FromAccount"].ToString();
            FilterConditions["ToAccount"] = item => item.RecipientAccountNumber == FilterValues["ToAccount"].ToString();
            //FilterConditions["FromDate"] = item => item.RecordedOn > FilterValues["FromDate"];
            //FilterConditions["ToDate"] = item => item.RecordedOn < FilterValues["ToDate"];
        }



        public IEnumerable<TransactionBObj> AllTransactions { get; set; } = new List<TransactionBObj>();

        public IEnumerable<TransactionBObj> FilteredTransactions { get; set; } = new List<TransactionBObj>();

        public void LoadAllTransactionsData()
        {
            GetAllTransactionsRequest request = new GetAllTransactionsRequest()
            {
                CustomerID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsPresenterCallback(this);
            UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateTransactionsData(TransactionPageDataUpdatedArgs args)
        {
            AllTransactions = args.TransactionList;
            UpdateSelectedAccount(SelectedAccount);
        }

        private void UpdateOnViewList()
        {
            int startIndex = (CurrentPageIndex) * RowsPerPage;
            InViewTransactions = new ObservableCollection<TransactionBObj>(FilteredTransactions.Skip(startIndex).Take(RowsPerPage));
            UpdatePageNavigation();
            CalculateTotalPages();
        }

        private void UpdatePageNavigation()
        {
            (NextCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCommand as RelayCommand).RaiseCanExecuteChanged();
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
            LoadAllTransactionsData();
        }

        private void PaneClosed(FrameworkElement obj)
        {
            if(obj == null)
            {
                InViewTransaction = null;
            }
        }

        internal void UpdateSelectedAccount(AccountBObj accountBObj)
        {
            SelectedAccount = accountBObj;
            FilteredTransactions = AllTransactions.Where(trans => trans.SenderAccountNumber == SelectedAccount?.AccountNumber || trans.RecipientAccountNumber == SelectedAccount?.AccountNumber);
            UpdateOnViewList();
        }

        private void ResetFilterValues()
        {
            FilterValues["FromAccount"] = null;
            FilterValues["ToAccount"] = null;
            FilterValues["FromDate"] = null;
            FilterValues["ToDate"] = null;
            FilterValues["TransactionType"] = null;
        }


        public void ApplyFilter()
        {

        }


        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            AccountsList = new ObservableCollection<AccountBObj>(args.AccountsList);
            if (AccountsList.Count > 0)
            {
                UpdateSelectedAccount(AccountsList.ElementAt(0));
            }
        }

        private ObservableCollection<AccountBObj> _accountsList = new ObservableCollection<AccountBObj>();

        public ObservableCollection<AccountBObj> AccountsList
        {
            get { return _accountsList; }
            set { Set(ref  _accountsList, value); } 
        }

        private AccountBObj _selectedAccount { get; set; }

        public AccountBObj SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
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


        private void NewTransactionAdded(bool isPaymentCompleted)
        {
            if (isPaymentCompleted)
            {
                LoadAllTransactionsData();
            }
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactionsData;
            ViewNotifier.Instance.CancelPaymentRequested -= NewTransactionAdded;
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.RightPaneContentUpdated -= PaneClosed;
        }

        private ObservableDictionary<string, object> _filterValues { get; set; }

        public ObservableDictionary<string, object> FilterValues
        {
            get
            {
                return _filterValues;
            }
            set
            {
                _filterValues = value;
                OnPropertyChanged(nameof(FilterValues));
            }
        }

        private int _currentPageIndex { get; set; }

        private int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }
            set
            {
                _currentPageIndex = value;
                CurrentPage = value + 1;
                OnPropertyChanged(nameof(CurrentPageIndex));
            }
        }

        private int _currentPage { get; set; }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private void CalculateTotalPages()
        {
            TotalPages = (FilteredTransactions.Count() / RowsPerPage);
            if (FilteredTransactions.Count() % RowsPerPage != 0)
            {
                TotalPages += 1;
            }
        }

        private IDictionary<string, Func<TransactionBObj, bool>> FilterConditions = new Dictionary<string, Func<TransactionBObj, bool>>();
            


        private bool IsPreviousButtonEnabled()
        {
            return CurrentPageIndex > 0;
        }

        private bool IsNextButtonEnabled()
        {
            return CurrentPageIndex  < (FilteredTransactions.Count() / RowsPerPage);
        }

        private void GoToPreviousPage(object parameter)
        {
            CurrentPageIndex--;
            UpdateOnViewList();
            UpdatePageNavigation();
        }

        private void GoToNextPage(object parameter)
        {
            CurrentPageIndex++;
            UpdateOnViewList();
            UpdatePageNavigation();
        }

        internal void UpdateView(TransactionBObj transaction)
        {
            InViewTransaction = transaction;
            ViewTransaction viewTransaction = new ViewTransaction();
            viewTransaction.InViewTransaction = transaction;
            ViewNotifier.Instance.OnRightPaneContentUpdated(viewTransaction);
        }

        private int _totalPages;
        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                _totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

    
        private int _rowsPerPage { get; set; }

        public int RowsPerPage
        {
            get { return _rowsPerPage; }
            set
            {
                _rowsPerPage = value;
                OnPropertyChanged(nameof(RowsPerPage));
                CalculateTotalPages();
                CurrentPageIndex = 0;
                UpdateOnViewList();
            }
        }

        private ObservableCollection<TransactionBObj> _inViewTransactions { get; set; }

        public ObservableCollection<TransactionBObj> InViewTransactions
        {
            get { return _inViewTransactions; }
            set
            {
                _inViewTransactions = value;
                OnPropertyChanged(nameof(InViewTransactions));
            }
        }

        private TransactionBObj _inViewTransaction { get; set; }

        public TransactionBObj InViewTransaction
        {
            get { return _inViewTransaction; }
            set
            {
                _inViewTransaction = value;
                OnPropertyChanged(nameof(InViewTransaction));
            }
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
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                    {
                        TransactionList = response.Transactions,
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
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
