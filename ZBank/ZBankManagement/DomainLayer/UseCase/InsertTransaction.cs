using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class InsertTransaction
    {
        public class InsertTransactionUseCase : UseCaseBase<InsertTransactionRequest, InsertTransactionResponse>
        {
            private readonly IInsertTransactionDataManager InsertTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertTransactionDataManager>();

            private IPresenterCallback<InsertTransactionResponse> PresenterCallback;

            protected override void Action(InsertTransactionRequest request, IPresenterCallback<InsertTransactionResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                InsertTransactionDataManager.InsertTransaction(request, new InsertTransactionCallback(this));
            }

            private class InsertTransactionCallback : IUseCaseCallback<InsertTransactionResponse>
            {

                InsertTransactionUseCase UseCase;

                public InsertTransactionCallback(InsertTransactionUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(InsertTransactionResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertTransactionRequest
        {
            public Transaction TransactionToInsert { get; set; }
        }

        public class InsertTransactionResponse
        {
            public bool IsSuccess { get; set; }

            public Transaction InsertedTransaction { get; set; }
        }

        public class InsertTransactionPresenterCallback : IPresenterCallback<InsertTransactionResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertTransactionPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(InsertTransactionResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                });
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
