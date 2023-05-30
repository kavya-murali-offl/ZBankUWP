using ZBankManagement.Data;
using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;
using System;
using ZBank.Entities.BusinessObjects;

namespace ZBankManagement.DataManager
{
    public class GetTransactionDataManager : IGetTransactionDataManager
    {
        public GetTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public void GetTransactionsByCustomerID(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                var accounts = DBHandler.GetAllAccounts(request.CustomerID).Result;
                List<TransactionBObj> transactions = new List<TransactionBObj>();    
                foreach (var account in accounts)
                {
                    var accountTransactions = DBHandler.GetTransactionByAccountNumber(account.AccountNumber).Result;
                    transactions.AddRange(accountTransactions);
                }

                IEnumerable<Beneficiary> beneficiaries = DBHandler.GetBeneficiaries(request.CustomerID).Result;

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

        public void GetTransactionsByAccountNumber(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                IEnumerable<TransactionBObj> transactionList = DBHandler.GetTransactionByAccountNumber(request.AccountNumber).Result;
                IEnumerable<Beneficiary> beneficiaries = DBHandler.GetBeneficiaries(request.CustomerID).Result;

                GetAllTransactionsResponse response = new GetAllTransactionsResponse
                {
                    Transactions = transactionList,
                    Beneficiaries = beneficiaries
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
