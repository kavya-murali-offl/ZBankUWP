using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertTransactionUseCase : UseCaseBase<InsertTransactionResponse>
        {
            private readonly IInsertTransactionDataManager InsertTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertTransactionDataManager>();
            private readonly InsertTransactionRequest _request;

            public InsertTransactionUseCase(InsertTransactionRequest request, IPresenterCallback<InsertTransactionResponse> presenterCallback)
            : base(presenterCallback, request.Token)
            {
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
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertTransactionRequest : RequestObjectBase
        {
            public Transaction TransactionToInsert { get; set; }
        }

        public class InsertTransactionResponse
        {
            public bool IsSuccess { get; set; }

            public Transaction InsertedTransaction { get; set; }
        }


}
