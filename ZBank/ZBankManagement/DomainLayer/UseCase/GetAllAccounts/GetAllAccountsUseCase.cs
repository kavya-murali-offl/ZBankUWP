using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.Entities.EnumerationType;

namespace ZBank.ZBankManagement.UseCase.GetAllAccounts
{
    public class GetAllAccountsUseCase : UseCaseBase<GetAllAccountsRequest, GetAllAccountsResponse>
    {
        private readonly IGetAccountDataManager GetAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetAccountDataManager>();
        
        private IPresenterCallback<GetAllAccountsResponse> PresenterCallback;

        protected override void Action(GetAllAccountsRequest request , IPresenterCallback<GetAllAccountsResponse> presenterCallback)
        {
            PresenterCallback = presenterCallback;
            GetAccountDataManager.GetAllAccounts(request, new GetAllAccountsCallback(this));
        }

        private class GetAllAccountsCallback : IUseCaseCallback<GetAllAccountsResponse>
        {

            GetAllAccountsUseCase UseCase;

            public GetAllAccountsCallback(GetAllAccountsUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(GetAllAccountsResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                UseCase.PresenterCallback.OnFailure(error);
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
}
