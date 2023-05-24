using ZBankManagement.Data;
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
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;

namespace ZBankManagement.DataManager
{
    public class GetAccountDataManager : IGetAccountDataManager
    {
        public GetAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void GetAllAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                IEnumerable<Account> accountsList = DBHandler.GetAllAccounts(request.UserID).Result;
                foreach(Account account in accountsList) { 
                    var transactions = DBHandler.GetTransactionByAccountNumber(account.AccountNumber).Result;
                    account.Transactions = transactions.Select(tx => Mapper.GetTransactionBObj(tx));
                    account.Branch = DBHandler.GetBranchByIfscCode(account.IFSCCode).Result.FirstOrDefault();
                    account.LinkedCard = DBHandler.GetCardByAccountNumber(account.AccountNumber).Result.FirstOrDefault();
                }
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

    }
}
