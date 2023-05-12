using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
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

    public class GetAllAccountsUseCase : UseCaseBase<GetAllAccountsResponse>
    {

        private readonly IGetAccountDataManager _getAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetAccountDataManager>();
        private readonly IPresenterCallback<GetAllAccountsResponse> _presenterCallback;
        private readonly GetAllAccountsRequest _request;

        public GetAllAccountsUseCase(GetAllAccountsRequest request, IPresenterCallback<GetAllAccountsResponse> presenterCallback) {
            _presenterCallback = presenterCallback;
            _request = request;
        }
        

        protected override void Action()
        {
            _getAccountDataManager.GetAllAccounts(_request, new GetAllAccountsCallback(this));
        }

        private class GetAllAccountsCallback : IUseCaseCallback<GetAllAccountsResponse>
        {

            private readonly GetAllAccountsUseCase _useCase;

            public GetAllAccountsCallback(GetAllAccountsUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllAccountsResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
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
            // Notify view
        }
    }
}
