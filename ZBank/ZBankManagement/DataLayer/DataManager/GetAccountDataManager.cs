using BankManagementDB.Data;
using BankManagementDB.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using System.Linq;
using ZBank.Entities.EnumerationType;
using ZBank.ZBankManagement.UseCase.GetAllAccounts;
using BankManagementDB.Domain.UseCase;
using System;
using ZBank.Entity.EnumerationTypes;

namespace BankManagementDB.DataManager
{
    public class GetAccountDataManager : IGetAccountDataManager
    {
        public GetAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public void GetAllAccounts(string customerId)
        {
            //IList<SavingsAccount> savingsAccounts = DBHandler.GetSavingsAccounts(customerId).Result;
            //IList<CurrentAccount> currentAccounts = DBHandler.GetCurrentAccounts(customerId).Result;
            //Store.AccountsList = new List<Account>();
            //Store.AccountsList = Store.AccountsList.Concat(currentAccounts);
            //Store.AccountsList = Store.AccountsList.Concat(savingsAccounts);
        }

        public void GetAllAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                IEnumerable<Account> accountsList = new List<Account>();
                if (request.AccountType == null)
                {
                    accountsList = DBHandler.GetAllAccounts(request.UserID).Result;
                    //IEnumerable<SavingsAccount> savingsAccounts = DBHandler.GetSavingsAccounts(request.userID).Result;
                    //IEnumerable<CurrentAccount> currentAccounts = DBHandler.GetCurrentAccounts(request.userID).Result;
                    //accountsList = accountsList.Concat(savingsAccounts);
                    //accountsList = accountsList.Concat(currentAccounts);
                }

                GetAllAccountsResponse response = new GetAllAccountsResponse()
                {
                    Accounts = accountsList
                };
                callback.OnSuccess(response);
            }
            catch(Exception ex)
            {
                ZBankError error = new ZBankError()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }
            
        }

    }
}
