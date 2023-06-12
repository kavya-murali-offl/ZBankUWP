using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;
using System;
using ZBank.Entities.BusinessObjects;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    public class GetTransactionDataManager : IGetTransactionDataManager
    {
        public GetTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public async Task GetTransactionsByCustomerID(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                var accounts = await DBHandler.GetAllAccounts(request.CustomerID);
                List<TransactionBObj> transactions = new List<TransactionBObj>();    
                foreach (var account in accounts)
                {
                    var accountTransactions = await DBHandler.GetTransactionByAccountNumber(account.AccountNumber);
                    transactions.AddRange(accountTransactions);
                }

                IEnumerable<Beneficiary> beneficiaries = await DBHandler.GetBeneficiaries(request.CustomerID);

                GetAllTransactionsResponse response = new GetAllTransactionsResponse
                {
                    Transactions = transactions,
                    Beneficiaries = beneficiaries,
                    Accounts = accounts
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

        public async Task GetTransactionsByAccountNumber(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                IEnumerable<TransactionBObj> transactionList = await DBHandler.GetAllTransactionByAccountNumber(request.AccountNumber);

                GetAllTransactionsResponse response = new GetAllTransactionsResponse
                {
                    Transactions = transactionList,
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
