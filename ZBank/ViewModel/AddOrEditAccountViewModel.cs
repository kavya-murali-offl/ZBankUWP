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

namespace ZBank.ViewModel
{
    public class AddOrEditAccountViewModel : ViewModelBase
    {
        public IView View { get; set; }

        public ICommand SubmitCommand { get; set; }

        public AddOrEditAccountViewModel(IView view)
        {
            View = view;
            SubmitCommand = new RelayCommand(SubmitAccount);
            LoadAllAccounts();
        }

        private void SubmitAccount(object parameter)
        {
            Account account = (Account)parameter;
            if(account.AccountNumber == null)
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
        }

        public void UnloadContent()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            Accounts = new ObservableCollection<Account>(args.AccountsList);
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

        public void ApplyNewAccount(Account accountToInsert)
        {
             //accountToInsert.Balance = 0;
             //accountToInsert.UserID = 

            InsertAccountRequest request = new InsertAccountRequest()
            {
               AccountToInsert = accountToInsert
            };

            IPresenterCallback<InsertAccountResponse> presenterCallback = new InsertAccountPresenterCallback(this);
            UseCaseBase<InsertAccountResponse> useCase = new InsertAccountUseCase(request, presenterCallback);
            useCase.Execute();

        }
    }
}
