using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllTransactionsUseCase : UseCaseBase<GetAllTransactionsRequest, GetAllTransactionsResponse>
    {
        private readonly IGetTransactionDataManager GetTransactionDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetTransactionDataManager>();

        private IPresenterCallback<GetAllTransactionsResponse> PresenterCallback;

        protected override void Action(GetAllTransactionsRequest request, IPresenterCallback<GetAllTransactionsResponse> presenterCallback)
        {
            PresenterCallback = presenterCallback;
            GetTransactionDataManager.GetTransactionsByAccountNumber(request, new GetAllTransactionsCallback(this));
        }

        private class GetAllTransactionsCallback : IUseCaseCallback<GetAllTransactionsResponse>
        {

            GetAllTransactionsUseCase UseCase;

            public GetAllTransactionsCallback(GetAllTransactionsUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(GetAllTransactionsResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                UseCase.PresenterCallback.OnFailure(error);
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
        private AccountPageViewModel AccountPageViewModel { get; set; }

        public GetAllTransactionsPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
            AccountPageViewModel = accountPageViewModel;
        }

        public async void OnSuccess(GetAllTransactionsResponse response)
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
