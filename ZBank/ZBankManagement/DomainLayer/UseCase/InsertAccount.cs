using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Linq;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Notifications;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.ZBankManagement.AppEvents.AppEventArgs;
using ZBank.ZBankManagement.AppEvents;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertAccountUseCase : UseCaseBase<InsertAccountResponse>
        {
            private readonly IInsertAccountDataManager _insertAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertAccountDataManager>();
            private readonly InsertAccountRequest _request;

            public InsertAccountUseCase(InsertAccountRequest request, IPresenterCallback<InsertAccountResponse> presenterCallback) : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _insertAccountDataManager.InsertAccount(_request, new InsertAccountCallback(this));
            }

            private class InsertAccountCallback : IUseCaseCallback<InsertAccountResponse>
            {

                private InsertAccountUseCase _useCase;

                public InsertAccountCallback(InsertAccountUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertAccountResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertAccountRequest : RequestObjectBase
        {
            public Account AccountToInsert { get; set; }
        }

        public class InsertAccountResponse
        {
            public bool IsSuccess { get; set; }

            public Account InsertedAccount { get; set; }
        }

        public class InsertAccountPresenterCallback : IPresenterCallback<InsertAccountResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(InsertAccountResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (AccountPageViewModel.Accounts != null)
                    {
                        AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                        {
                            AccountsList = new ObservableCollection<Account>(AccountPageViewModel.Accounts.Append(response.InsertedAccount))   
                        };
                        ViewNotifier.Instance.OnAccountsListUpdated(args);
                    }
                });
            }

            public void OnFailure(ZBankException error)
            {
                
            }
        }
}
