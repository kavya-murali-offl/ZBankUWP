using BankManagementDB.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.UseCase.GetAllAccounts;

namespace ZBank.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {

        public AccountPageViewModel()
        {
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


        //private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //        {
        //            NotifyPropertyChanged("Items");
        //        }
        //    }
    }
}
