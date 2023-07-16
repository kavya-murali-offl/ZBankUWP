using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using System.Linq;
using ZBank.Entities.EnumerationType;
using ZBankManagement.Domain.UseCase;
using System;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Diagnostics;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    class GetAccountDataManager : IGetAccountDataManager
    {
        public GetAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task GetAllAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                IEnumerable<AccountBObj> accountsList = await DBHandler.GetAllAccounts(request.UserID).ConfigureAwait(false);
                GetAllAccountsResponse response = new GetAllAccountsResponse()
                {
                    Accounts = accountsList
                };
                callback.OnSuccess(response);
            }
            catch(Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }
            
        }

        public async Task GetAllTransactionAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                IEnumerable<AccountBObj> accountsList = await DBHandler.GetAllAccounts(request.UserID).ConfigureAwait(false);
                var transactionAccounts = accountsList.Where(acc => acc.AccountType != AccountType.TERM_DEPOSIT);
                GetAllAccountsResponse response = new GetAllAccountsResponse()
                {
                    Accounts = transactionAccounts
                };
                callback.OnSuccess(response);
            }
            catch (Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }

        }

        public async Task GetAccountByAccountNumber(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                AccountBObj account = await DBHandler.GetAccountByAccountNumber(request.UserID, request.AccountNumber).ConfigureAwait(false);
                GetAllAccountsResponse response = new GetAllAccountsResponse()
                {
                    Accounts = new List<AccountBObj>()
                    {
                        account
                    }
                };
                callback.OnSuccess(response);
            }
            catch (Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }

        }

    }
}
