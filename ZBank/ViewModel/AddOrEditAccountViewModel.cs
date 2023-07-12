﻿using System;
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
using ZBank.Entity.EnumerationTypes;
using ZBank.DataStore;
using Windows.UI.ViewManagement;
using ZBank.Utilities.Helpers;
using ZBank.View.DataTemplates.NewAcountTemplates;

namespace ZBank.ViewModel
{
    public class AddOrEditAccountViewModel : ViewModelBase
    {

        public IList<AccountType> AccountTypes { get; set; } = new List<AccountType>() { AccountType.CURRENT, AccountType.SAVINGS, AccountType.TERM_DEPOSIT };
        
        public ICommand SubmitCommand { get; set; }
        
        public AccountType SelectedAccountType { get; set; }

        public AddOrEditAccountViewModel(IView view)
        {
            View = view;
            DispatcherManager.Current.Dispatcher = view.Dispatcher;
            BranchList = new ObservableCollection<Branch>();
            Reset();
        }

        public ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        public ObservableDictionary<string, object> FieldValues = new ObservableDictionary<string, object>();

        public void ValidateAndSubmit()
        {
            if (ValidateFields())
            {
                Account account = GetAccount();
                if (account.AccountNumber == null)
                {
                    IReadOnlyList<StorageFile> files = FieldValues["KYC"] as IReadOnlyList<StorageFile>;
                    if(files.Count > 0)
                    {
                        ApplyNewAccount(account, files);
                    }
                }
            }
        }

        private bool ValidateFields()
        {
            foreach (var key in FieldValues.Keys)
            {
                ValidateField(key);
            }

            var inText = FieldValues["Amount"].ToString();
            if (FieldErrors["Amount"] == string.Empty && decimal.TryParse(inText, out decimal amountInDecimal))
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
            FieldValues["Branch"] = null;
            FieldErrors["Branch"] = string.Empty;
            FieldValues["Amount"] = string.Empty;
            FieldErrors["Amount"] = string.Empty;
            FieldValues["KYC"] = null;
            FieldErrors["KYC"] = string.Empty;
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

        private Account GetAccount()
        {
            Account account = null;
            switch (SelectedAccountType)
            {
                case AccountType.CURRENT:
                   account =  new CurrentAccount()
                    {
                       AccountType = AccountType.CURRENT,
                        InterestRate = 3.2m,
                        MinimumBalance=500m
                    };
                    break;
                case AccountType.SAVINGS:
                    account = new SavingsAccount()
                    {
                        AccountType = AccountType.SAVINGS,
                        InterestRate = 4.8m,
                        MinimumBalance = 500m
                    };
                    break;
                case AccountType.TERM_DEPOSIT:
                    TermDepositAccount depositAccount = new TermDepositAccount()
                    {
                        Balance = decimal.Parse(FieldValues["Amount"].ToString()),
                        AccountType = AccountType.TERM_DEPOSIT,
                        Tenure = int.Parse(FieldValues["Tenure"].ToString()),
                        DepositType = DepositType.OnMaturity,
                        DepositStartDate = DateTime.Now,
                        RepaymentAccountNumber = Accounts.FirstOrDefault(acc => acc.ToString().Equals(FieldValues["Repayment Account Number"].ToString())).AccountNumber,
                        FromAccountNumber = Accounts.FirstOrDefault(acc => acc.ToString().Equals(FieldValues["From Account Number"].ToString())).AccountNumber,
                    };
                    depositAccount.SetDefault();
                    account = depositAccount;
                    break;
                 default: break;
            }
            if(account != null)
            {
                account.Balance = decimal.Parse(FieldValues["Amount"].ToString());
                account.IFSCCode = BranchList.FirstOrDefault(brnch => brnch.ToString().Equals(FieldValues["Branch"].ToString()))?.IfscCode;
                account.UserID = Repository.Current.CurrentUserID;
                account.AccountStatus = AccountStatus.ACTIVE;
                account.Balance = decimal.Parse(FieldValues["Amount"].ToString());
                account.CreatedOn = DateTime.Now;
                account.Currency = Currency.INR;
                account.AccountName="";
            }
            return account;
        }

       

        internal void LoadContent()
        {
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.BranchListUpdated += UpdateBranchesList;
            ViewNotifier.Instance.AccountInserted += OnAccountInsertionSuccessful;
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
            LoadAllAccounts();
            LoadAllBranches();
        }

        private void OnAccountInsertionSuccessful(bool isInserted)
        {
            WindowService.CloseWindow(CoreApplication.GetCurrentView());
        }

        private void ConsolidateView()
        {
            UnloadContent();
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }

        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ConsolidateView();
        }

        internal void UnloadContent()
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

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                IsTransactionAccounts = true,
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsInAddPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void LoadAllBranches()
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

        internal async Task UploadFiles()
        {
            var files = await FilesHelper.GetFiles();
            if(files.Count > 0)
            {
                FieldValues["KYC"] = files;
                FieldErrors["KYC"] = string.Empty;
                UploadInfo = $"Uploaded {files.Count} file(s)";
            }
            else
            {
                FieldErrors["KYC"] = "No files uploaded";
            }
        }

        internal void SwitchTemplate(AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.CURRENT:
                    NewCurrentAccountFormTemplate newCurrentAccountFormTemplate = new NewCurrentAccountFormTemplate()
                    {
                        SubmitCommand = SubmitCommand,
                        FieldErrors = FieldErrors,
                        FieldValues = FieldValues,
                    };
                    newCurrentAccountFormTemplate.Reset();
                    SelectedTemplate = newCurrentAccountFormTemplate;
                    break;
                case AccountType.SAVINGS:
                    NewSavingsAccountFormTemplate newSavingsAccountFormTemplate = new NewSavingsAccountFormTemplate()
                    {
                        SubmitCommand = SubmitCommand,
                        FieldErrors = FieldErrors,
                        FieldValues = FieldValues,
                    };
                    newSavingsAccountFormTemplate.Reset();
                    SelectedTemplate = newSavingsAccountFormTemplate;
                    break;
                case AccountType.TERM_DEPOSIT:
                    NewDepositAccountFormTemplate newDepositAccountFormTemplate = new NewDepositAccountFormTemplate()
                    {
                        SubmitCommand = SubmitCommand,
                        FieldErrors = FieldErrors,
                        FieldValues = FieldValues,
                    };
                    newDepositAccountFormTemplate.Reset();
                    SelectedTemplate = newDepositAccountFormTemplate;   
                    break;
            }
        }

        private Customer _currentCustomer = null;

        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                _currentCustomer = value;
                OnPropertyChanged(nameof(CurrentCustomer));
            }
        }

        private string _uploadInfo = string.Empty;

        public string UploadInfo
        {
            get { return _uploadInfo; }
            set
            {
                _uploadInfo = value;
                OnPropertyChanged(nameof(UploadInfo));
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

        private FrameworkElement _selectedTemplate { get; set; }

        public FrameworkElement SelectedTemplate
        {
            get { return _selectedTemplate; }
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged(nameof(SelectedTemplate));
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

        private class InsertAccountPresenterCallback : IPresenterCallback<InsertAccountResponse>
        {
            private AddOrEditAccountViewModel ViewModel { get; set; }

            public InsertAccountPresenterCallback(AddOrEditAccountViewModel accountPageViewModel)
            {
                ViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(InsertAccountResponse response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnAccountInserted(true);
                });
            }

            public async Task OnFailure(ZBankException error)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(
                        new Notification()
                        {
                            Message = error.Message,
                            Type = NotificationType.ERROR
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
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
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(
                      new Notification()
                      {
                            Message = response.Message,
                            Type = NotificationType.ERROR
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
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
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
                    ViewNotifier.Instance.OnNotificationStackUpdated(
                      new Notification()
                      {
                          Message = response.Message,
                          Type = NotificationType.ERROR
                      });
                });
            }
        }
    }
}
