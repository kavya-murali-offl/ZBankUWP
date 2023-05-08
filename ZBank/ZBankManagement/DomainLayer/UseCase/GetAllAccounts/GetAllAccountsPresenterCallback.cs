using BankManagementDB.Domain.UseCase;
using BankManagementDB.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
    {
        private AccountPageViewModel AccountPageViewModel { get; set; }

        public GetAllAccountsPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
            AccountPageViewModel = accountPageViewModel;
        }

        public void OnSuccess(GetAllAccountsResponse response)
        {
            AccountPageViewModel.Accounts = new ObservableCollection<Account>(response.Accounts);
        }

        public void OnFailure(ZBankError response)
        {

        }
    }
}
