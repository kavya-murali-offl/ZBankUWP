using BankManagementDB.Domain.UseCase;
using System.Collections.ObjectModel;
using ZBank.Entities;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBank.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {
        public IView View;

        public AccountPageViewModel(IView view)
        {
            View = view;
            LoadAllAccounts();
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

        public void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                UserID = "1111"
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);

            UseCaseBase<GetAllAccountsRequest, GetAllAccountsResponse> useCase = new GetAllAccountsUseCase();

            useCase.Execute(request, presenterCallback);
        }
    }
}
