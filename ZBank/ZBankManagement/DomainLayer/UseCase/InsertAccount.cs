using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using System;
using System.Linq;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class InsertAccount
    {
        public class InsertAccountUseCase : UseCaseBase<InsertAccountRequest, InsertAccountResponse>
        {
            private readonly IInsertAccountDataManager InsertAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertAccountDataManager>();

            private IPresenterCallback<InsertAccountResponse> PresenterCallback;

            protected override void Action(InsertAccountRequest request, IPresenterCallback<InsertAccountResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                InsertAccountDataManager.InsertAccount(request, new InsertAccountCallback(this));
            }

            private class InsertAccountCallback : IUseCaseCallback<InsertAccountResponse>
            {

                InsertAccountUseCase UseCase;

                public InsertAccountCallback(InsertAccountUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(InsertAccountResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertAccountRequest
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
                    var accounts = AccountPageViewModel.Accounts.Prepend(response.InsertedAccount);
                    AccountPageViewModel.Accounts = new ObservableCollection<Account>(accounts);
                });
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
