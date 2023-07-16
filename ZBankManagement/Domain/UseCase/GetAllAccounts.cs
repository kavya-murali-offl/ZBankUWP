using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using ZBank.Dependencies;
using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
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
            if (_request.IsTransactionAccounts)
            {
                _getAccountDataManager.GetAllTransactionAccounts(_request, new GetAllAccountsCallback(this));
            }
            else if (_request.AccountNumber != null)
            {
                _getAccountDataManager.GetAccountByAccountNumber(_request, new GetAllAccountsCallback(this));
            }
            else
            {
                _getAccountDataManager.GetAllAccounts(_request, new GetAllAccountsCallback(this));
            }
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

        public bool IsTransactionAccounts { get; set; }
        public string AccountNumber { get; set; }

        public string UserID { get; set; }
    }


    public class GetAllAccountsResponse
    {
        public IEnumerable<AccountBObj> Accounts { get; set; }
    }
}
