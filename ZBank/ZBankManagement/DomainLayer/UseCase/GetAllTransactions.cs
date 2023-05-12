using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllTransactionsUseCase : UseCaseBase<GetAllTransactionsResponse>
    {

        private readonly IGetTransactionDataManager _getTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetTransactionDataManager>();
        private readonly IPresenterCallback<GetAllTransactionsResponse> _presenterCallback;
        private readonly GetAllTransactionsRequest _request;

        public GetAllTransactionsUseCase(GetAllTransactionsRequest request, IPresenterCallback<GetAllTransactionsResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
            _request = request;
        }

        protected override void Action()
        {
            _getTransactionDataManager.GetTransactionsByAccountNumber(_request, new GetAllTransactionsCallback(this));
        }

        private class GetAllTransactionsCallback : IUseCaseCallback<GetAllTransactionsResponse>
        {
            private readonly GetAllTransactionsUseCase _useCase;

            public GetAllTransactionsCallback(GetAllTransactionsUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllTransactionsResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllTransactionsRequest
    {
        public string AccountNumber { get; set; }
    }

    public class GetAllTransactionsResponse
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }


    public class GetAllTransactionsPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
    {

        public void OnSuccess(GetAllTransactionsResponse response)
        {
        }

        public void OnFailure(ZBankError response)
        {

        }
    }
}
