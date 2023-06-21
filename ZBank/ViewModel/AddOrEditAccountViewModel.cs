using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using System.Windows.Input;
using ZBank.ViewModel.VMObjects;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBankManagement.AppEvents.AppEventArgs;
using Windows.UI.Xaml;
using ZBank.Entities.EnumerationType;
using Windows.Storage;
using ZBank.Services;

namespace ZBank.ViewModel
{
    public class AddOrEditAccountViewModel : ViewModelBase, IForm
    {
        public IView View { get; set; }
        public IList<AccountType> AccountTypes { get; set; } = new List<AccountType>() { AccountType.CURRENT, AccountType.SAVINGS, AccountType.TERM_DEPOSIT };
        public ICommand SubmitCommand { get; set; }
        public AccountType SelectedAccountType { get; set; }

        public AddOrEditAccountViewModel(IView view)
        {
            View = view;
            SubmitCommand = new RelayCommand(GetAccount);
            BranchList = new ObservableCollection<Branch>();
            Reset();
        }

        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, object> FieldValues = new ObservableDictionary<string, object>();

        public void ValidateAndSubmit()
        {
            if (ValidateFields())
            {
                CurrentAccount currentAccount = new CurrentAccount()
                {
                    IFSCCode = (FieldValues["Branch"] as Branch).IfscCode.ToString(),
                    AccountName = "Kavya",
                    AccountStatus = Entities.EnumerationType.AccountStatus.INACTIVE,
                    Balance = decimal.Parse(FieldValues["Amount"].ToString()),
                    AccountType = Entities.EnumerationType.AccountType.CURRENT,
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                };
                SubmitCommand.Execute(currentAccount);
            }
        }
        private bool ValidateFields()
        {
            foreach (var key in FieldValues.Keys)
            {
                ValidateField(key);
            }

            var inText = FieldValues["Amount"].ToString();
            if (decimal.TryParse(inText, out decimal amountInDecimal))
            {
                FieldErrors["Amount"] = string.Empty;
            }
            else
            {
                FieldErrors["Amount"] = "Please enter a valid deposit Amount";
            }

            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;
            return true;
        }

        public void Reset()
        {
            FieldValues["Branch"] = string.Empty;
            FieldErrors["Branch"] = string.Empty;
            FieldValues["Amount"] = string.Empty;
            FieldErrors["Amount"] = string.Empty;
        }

        public void ValidateField(string fieldName)
        {
            if (!FieldValues.TryGetValue(fieldName, out object val) || string.IsNullOrEmpty(FieldValues[fieldName]?.ToString()))
            {
                FieldErrors[fieldName] = $"{fieldName} is required.";
            }
            else
            {
                FieldErrors[fieldName] = string.Empty;
            }
        }

        private void GetAccount(object parameter)
        {
            Account account = (Account)parameter;
            if (account.IFSCCode == null)
            {

            }
            else
            {
                if (account.AccountNumber == null)
                {
                    IReadOnlyList<StorageFile> files = FieldValues["KYC"] as IReadOnlyList<StorageFile>;
                    ApplyNewAccount(account, files);
                }
                else
                {
                    // edit account
                }
            }
        }

        private ObservableCollection<Account> _accounts { get; set; }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private ObservableCollection<Branch> _branchList { get; set; }

        public ObservableCollection<Branch> BranchList
        {
            get { return _branchList; }
            set
            {
                _branchList = value;
                OnPropertyChanged(nameof(BranchList));
            }
        }


        public IEnumerable<DropDownItem> TenureList { get; set; } = new List<DropDownItem>()
        {
            new DropDownItem("6 months", 3),
            new DropDownItem("1 year", 12),
            new DropDownItem("2 years", 24),
        };

        public bool IsEdit { get; set; }

        public void LoadContent()
        {
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.BranchListUpdated += UpdateBranchesList;
            LoadAllAccounts();
            LoadAllBranches();
        }

        public async void CloseView()
        {
            await _viewLifetimeControl.CloseAsync();
        }

        public void UnloadContent()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.BranchListUpdated -= UpdateBranchesList;
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            Accounts = new ObservableCollection<Account>(args.AccountsList);
        }

        private void UpdateBranchesList(BranchListUpdatedArgs args)
        {
            BranchList = new ObservableCollection<Branch>(args.BranchList);
        }

        public void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                UserID = "1111"
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsInAddPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void LoadAllBranches()
        {
            GetAllBranchesRequest request = new GetAllBranchesRequest();

            IPresenterCallback<GetAllBranchesResponse> presenterCallback = new GetAllBranchesPresenterCallback(this);
            UseCaseBase<GetAllBranchesResponse> useCase = new GetAllBranchesUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void ApplyNewAccount(Account accountToInsert, IReadOnlyList<StorageFile> files)
        {
            InsertAccountRequest request = new InsertAccountRequest()
            {
                AccountToInsert = accountToInsert,
                Documents = files
            };

            IPresenterCallback<InsertAccountResponse> presenterCallback = new InsertAccountPresenterCallback(this);
            UseCaseBase<InsertAccountResponse> useCase = new InsertAccountUseCase(request, presenterCallback);
            useCase.Execute();
        }

        internal void UpdateBranch(Branch branch)
        {
            FieldValues["Branch"] = branch;
            if (branch != null)
            {
                FieldErrors["Branch"] = string.Empty;
            }
        }

        private ViewLifetimeControl _viewLifetimeControl;

        public void Initialize(ViewLifetimeControl viewLifetimeControl)
        {
            _viewLifetimeControl = viewLifetimeControl;
            _viewLifetimeControl.Released += OnViewLifetimeControlReleased;
        }

        private async void OnViewLifetimeControlReleased(object sender, EventArgs e)
        {
            _viewLifetimeControl.Released -= OnViewLifetimeControlReleased;
            await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                WindowManagerService.Current.SecondaryViews.Remove(_viewLifetimeControl);
            });
        }

        private class InsertAccountPresenterCallback : IPresenterCallback<InsertAccountResponse>
        {
            private AddOrEditAccountViewModel ViewModel { get; set; }

            public InsertAccountPresenterCallback(AddOrEditAccountViewModel accountPageViewModel)
            {
                ViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(InsertAccountResponse response)
            {
                await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnAccountInserted(true);
                });

                
            }

            public async Task OnFailure(ZBankException error)
            {
                await WindowManagerService.Current.MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = error.Message,
                            Type = NotificationType.ERROR
                        }
                    });
                });
            }
        }

        private class GetAllBranchesPresenterCallback : IPresenterCallback<GetAllBranchesResponse>
        {
            private AddOrEditAccountViewModel ViewModel { get; set; }

            public GetAllBranchesPresenterCallback(AddOrEditAccountViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllBranchesResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    BranchListUpdatedArgs args = new BranchListUpdatedArgs()
                    {
                        BranchList = response.BranchList
                    };
                    ViewNotifier.Instance.OnBranchListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Type = NotificationType.ERROR
                        }
                    });
                });

            }

        }

        private class GetAllAccountsInAddPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private AddOrEditAccountViewModel ViewModel { get; set; }

            public GetAllAccountsInAddPresenterCallback(AddOrEditAccountViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
                // Notify view
            }
        }
    }
}
