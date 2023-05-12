using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
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
        public class InsertTransactionUseCase : UseCaseBase<InsertTransactionResponse>
        {
            private readonly IInsertTransactionDataManager InsertTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertTransactionDataManager>();
            private readonly IPresenterCallback<InsertTransactionResponse> _presenterCallback;
            private readonly InsertTransactionRequest _request;

            public InsertTransactionUseCase(InsertTransactionRequest request, IPresenterCallback<InsertTransactionResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
                _request = request;
            }

            protected override void Action()
            {
                InsertTransactionDataManager.InsertTransaction(_request, new InsertTransactionCallback(this));
            }

            private class InsertTransactionCallback : IUseCaseCallback<InsertTransactionResponse>
            {
                readonly InsertTransactionUseCase _useCase;

                public InsertTransactionCallback(InsertTransactionUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertTransactionResponse response)
                {
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
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
