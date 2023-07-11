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
using System.Linq;
using System.Security.Principal;
using System.Transactions;

namespace ZBankManagement.DataManager
{
    class GetTransactionDataManager : IGetTransactionDataManager
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
                    var accountTransactions = await DBHandler.GetAllTransactionByAccountNumber(account.AccountNumber, request.CustomerID).ConfigureAwait(false);
                    foreach (var transaction in accountTransactions)
                    {
                        if (transaction.RecipientAccountNumber == account.AccountNumber)
                        {
                            transaction.IsRecipient = true;
                        }
                        transactions.Add(transaction);
                    }
                }

                IEnumerable<Beneficiary> beneficiaries = await DBHandler.GetBeneficiaries(request.CustomerID);

                GetAllTransactionsResponse response = new GetAllTransactionsResponse
                {
                    Transactions = transactions,
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

        public async Task GetTransactionsIncrementally(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                IEnumerable<TransactionBObj> accountTransactions = new List<TransactionBObj>();
                if (string.IsNullOrEmpty(request.AccountNumber))
                {
                    var accounts = await DBHandler.GetAllAccounts(request.CustomerID).ConfigureAwait(false);
                    foreach (var account in accounts)
                    {
                        var transactions = await DBHandler.GetAllTransactionByAccountNumber(account.AccountNumber, request.CustomerID).ConfigureAwait(false);
                        accountTransactions = from transaction in transactions  select transaction;
                    }
                }
                else
                {
                    accountTransactions = await DBHandler.GetAllTransactionByAccountNumber(request.AccountNumber, request.CustomerID).ConfigureAwait(false);
                }

                int totalPages = (accountTransactions.Count() / request.RowsPerPage);
                if (accountTransactions.Count() % request.RowsPerPage != 0)
                {
                    totalPages += 1;
                }
                if (totalPages == 0) totalPages += 1;
                accountTransactions = accountTransactions.Skip(request.CurrentPageIndex * request.RowsPerPage).Take(request.RowsPerPage);

                foreach (var transaction in accountTransactions)
                {
                    if (transaction.RecipientAccountNumber == request.AccountNumber)
                    {
                        transaction.IsRecipient = true;
                    }
                }
               
                GetAllTransactionsResponse response = new GetAllTransactionsResponse
                {
                    Transactions = accountTransactions,
                    TotalPages = totalPages
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
                IEnumerable<TransactionBObj> transactionList = await DBHandler.GetAllTransactionByAccountNumber(request.AccountNumber, request.CustomerID);
                foreach (var transaction in transactionList)
                {
                    if (transaction.RecipientAccountNumber == request.AccountNumber)
                    {
                        transaction.IsRecipient = true;
                    }
                }
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
