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

namespace ZBank.ViewModel
{
    public class AddOrEditAccountViewModel : ViewModelBase
    {
        public IView View { get; set; }

        public ICommand SubmitCommand { get; set; }

        public AddOrEditAccountViewModel(IView view)
        {
            View = view;
            SubmitCommand = new RelayCommand(GetAccount);
            BranchList = new ObservableCollection<Branch>();
        }

        private void GetAccount(object parameter)
        {
            Account account = (Account)parameter;
            if (account.AccountNumber == null)
            {
                ApplyNewAccount(account);
            }
            else
            {
                // edit account
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

        public SavingsAccount SavingsAccount { get; set; } = new SavingsAccount();
        public CurrentAccount CurrentAccount { get; set; } = new CurrentAccount();
        public TermDepositAccount DepositAccount { get; set; } = new TermDepositAccount();

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

        public void ApplyNewAccount(Account accountToInsert)
        {
            InsertAccountRequest request = new InsertAccountRequest()
            {
                AccountToInsert = accountToInsert
            };

            IPresenterCallback<InsertAccountResponse> presenterCallback = new InsertAccountPresenterCallback(this);
            UseCaseBase<InsertAccountResponse> useCase = new InsertAccountUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class InsertAccountPresenterCallback : IPresenterCallback<InsertAccountResponse>
        {
            private AddOrEditAccountViewModel AccountPageViewModel { get; set; }

            public InsertAccountPresenterCallback(AddOrEditAccountViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(InsertAccountResponse response)
            {
                await CoreApplication.GetCurrentView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnSuccessfulEvent(true);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = "Account Inserted Successfully",
                            Type = NotificationType.SUCCESS,
                        }
                    });
                });
            }

            public async Task OnFailure(ZBankException error)
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
                await CoreApplication.GetCurrentView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
