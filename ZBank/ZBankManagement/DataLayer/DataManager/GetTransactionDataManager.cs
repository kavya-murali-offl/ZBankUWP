using ZBankManagement.Data;
using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

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
            IEnumerable<Transaction> result = DBHandler.GetTransactionByAccountNumber(request.AccountNumber).Result;
          
            GetAllTransactionsResponse response = new GetAllTransactionsResponse();
            response.Transactions = result;

            callback.OnSuccess(response);
        }

        public IEnumerable<Transaction> GetTransactionsByCardNumber(string cardNumber) => DBHandler.GetTransactionByCardNumber(cardNumber).Result;
    }
}
