using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using ZBank.Dependencies;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.ViewModel;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities.BusinessObjects;
using Windows.ApplicationModel.Core;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{

    public class GetAllAccountsUseCase : UseCaseBase<GetAllAccountsResponse>
    {

        private readonly IGetAccountDataManager _getAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetAccountDataManager>();
        private readonly GetAllAccountsRequest _request;

        public GetAllAccountsUseCase(GetAllAccountsRequest request, IPresenterCallback<GetAllAccountsResponse> presenterCallback) 
            : base(presenterCallback, request.Token){
            _request = request;
        }
        

        protected override void Action()
        {
            _getAccountDataManager.GetAllAccounts(_request, new GetAllAccountsCallback(this));
        }

        private class GetAllAccountsCallback : IUseCaseCallback<GetAllAccountsResponse>
        {
            private readonly UseCaseBase<GetAllAccountsResponse> _useCase;

            public GetAllAccountsCallback(GetAllAccountsUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllAccountsResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllAccountsRequest : RequestObjectBase
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
                AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                {
                    AccountsList = new ObservableCollection<Account>(response.Accounts)
                };
                ViewNotifier.Instance.OnAccountsListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
        }
    }

    public class GetAllAccountsInDashboardPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
    {
        private DashboardViewModel DashboardViewModel { get; set; }

        public GetAllAccountsInDashboardPresenterCallback(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel = dashboardViewModel;
        }

        public async void OnSuccess(GetAllAccountsResponse response)
        {
            await DashboardViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                {
                    AccountsList = new ObservableCollection<Account>(response.Accounts)
                };
                ViewNotifier.Instance.OnAccountsListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
            // Notify view
        }
    }

    public class GetAllAccountsInAddPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
    {
        private AddOrEditAccountViewModel ViewModel { get; set; }

        public GetAllAccountsInAddPresenterCallback(AddOrEditAccountViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllAccountsResponse response)
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

        public void OnFailure(ZBankException response)
        {
            // Notify view
        }
    }

}
