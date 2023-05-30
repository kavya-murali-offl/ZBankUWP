using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using System.Collections.ObjectModel;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllTransactionsUseCase : UseCaseBase<GetAllTransactionsResponse>
    {

        private readonly IGetTransactionDataManager _getTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetTransactionDataManager>();
        private readonly GetAllTransactionsRequest _request;

        public GetAllTransactionsUseCase(GetAllTransactionsRequest request, IPresenterCallback<GetAllTransactionsResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }

        protected override void Action()
        {
            _getTransactionDataManager.GetTransactionsByCustomerID(_request, new GetAllTransactionsCallback(this));
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
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllTransactionsRequest : RequestObjectBase
    {
        public string CustomerID { get; set; }

        public string AccountNumber { get; set; }
    }

    public class GetAllTransactionsResponse
    {
        public IEnumerable<TransactionBObj> Transactions { get; set; }

        public IEnumerable<Beneficiary> Beneficiaries { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }


    public class GetAllTransactionsPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
    {
        public TransactionViewModel ViewModel { get; set; }

        public GetAllTransactionsPresenterCallback(TransactionViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllTransactionsResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                {
                    TransactionList = response.Transactions,
                    BeneficiariesList = response.Beneficiaries
                };

                ViewNotifier.Instance.OnTransactionsListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
        }
    }
}

