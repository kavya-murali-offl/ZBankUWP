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

namespace ZBank.ViewModel
{
    public class TransactionViewModel : ViewModelBase
    {
        public IView View;
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public IList<int> DefinedRows { get; private set; }

        public TransactionViewModel(IView view)
        {
            View = view;
            DefinedRows = new List<int>()
            {
               1, 5, 10, 25, 50
            };
            NextCommand = new RelayCommand(GoToNextPage, IsNextButtonEnabled);
            PreviousCommand = new RelayCommand(GoToPreviousPage, IsPreviousButtonEnabled);
            RowsPerPage = DefinedRows.First();
        }

        private bool IsPreviousButtonEnabled()
        {
            return CurrentPageIndex > 0;
        }

        private bool IsNextButtonEnabled()
        {
            return CurrentPageIndex + 1 < FilteredTransactions.Count() / RowsPerPage;
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

        private int _totalPages;
        public int TotalPages
        {
            get { return _totalPages; }
            set { 
                _totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

        public IEnumerable<TransactionBObj> AllTransactions { get; set; }

        public IEnumerable<TransactionBObj> FilteredTransactions { get; set; } = new List<TransactionBObj>();
        
        public void LoadAllTransactionsData()
        {
            GetAllTransactionsRequest request = new GetAllTransactionsRequest()
            {
                CustomerID = "1111"
            };

            IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsPresenterCallback(this);
            UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateTransactionsData(TransactionPageDataUpdatedArgs args)
        {
            AllTransactions = args.TransactionList;
            FilteredTransactions = args.TransactionList;
            UpdateOnViewList();
            CalculateTotalPages();
        }

        private void UpdateOnViewList()
        {
            int startIndex = (CurrentPageIndex) * RowsPerPage;
            InViewTransactions = new ObservableCollection<TransactionBObj>(FilteredTransactions.Skip(startIndex).Take(RowsPerPage));
            UpdatePageNavigation();
        }

        private void UpdatePageNavigation()
        {
            (NextCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCommand as RelayCommand).RaiseCanExecuteChanged();
        }


        public void OnPageLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated += UpdateTransactionsData;
            CurrentPageIndex = 0;
            RowsPerPage = DefinedRows.First();
            LoadAllTransactionsData();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactionsData;
        }

        private int _currentPageIndex { get; set; }

        private int CurrentPageIndex { 
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

    }

    public class GetAllTransactionsPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
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
                    BeneficiariesList = response.Beneficiaries
                };

                ViewNotifier.Instance.OnTransactionsListUpdated(args);
            });
        }

        public async Task OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllBeneficiariesInTransactionsPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
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
