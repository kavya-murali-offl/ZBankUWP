using ZBankManagement.Data;
using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;
using System;

namespace ZBankManagement.DataManager
{
    public class GetTransactionDataManager : IGetTransactionDataManager
    {
        public GetTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public void GetTransactionsByAccountNumber(GetAllTransactionsRequest request, IUseCaseCallback<GetAllTransactionsResponse> callback)
        {
            try
            {
                IEnumerable<Transaction> transactionList = DBHandler.GetTransactionByAccountNumber(request.AccountNumber).Result;
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

        public IEnumerable<Transaction> GetTransactionsByCardNumber(string cardNumber) => DBHandler.GetTransactionByCardNumber(cardNumber).Result;
    }
}
