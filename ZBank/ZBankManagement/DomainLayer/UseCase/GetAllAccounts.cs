using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.ViewModel;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using ZBank.ViewModel.VMObjects;
using Windows.UI.Core;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{

    public class GetAllAccountsUseCase : UseCaseBase<GetAllAccountsRequest, GetAllAccountsResponse>
    {
        private readonly IGetAccountDataManager GetAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetAccountDataManager>();

        private IPresenterCallback<GetAllAccountsResponse> PresenterCallback;

        protected override void Action(GetAllAccountsRequest request, IPresenterCallback<GetAllAccountsResponse> presenterCallback)
        {
            PresenterCallback = presenterCallback;
            GetAccountDataManager.GetAllAccounts(request, new GetAllAccountsCallback(this));
        }

        private class GetAllAccountsCallback : IUseCaseCallback<GetAllAccountsResponse>
        {

            GetAllAccountsUseCase UseCase;

            public GetAllAccountsCallback(GetAllAccountsUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(GetAllAccountsResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                UseCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllAccountsRequest
    {
        public AccountType? AccountType { get; set; }

        public string UserID { get; set; }
    }

    public class GetAllAccountsResponse
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
    public class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
    {
        private AccountPageViewModel AccountPageViewModel { get; set; }

        public GetAllAccountsPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
            AccountPageViewModel = accountPageViewModel;
        }

        public async void OnSuccess(GetAllAccountsResponse response)
        {
            await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccountPageViewModel.Accounts = new ObservableCollection<Account>(response.Accounts);
            });
        }

        public void OnFailure(ZBankError response)
        {

        }
    }
}
