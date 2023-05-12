using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public class InsertTransactionDataManager : IInsertTransactionDataManager
    {
        public InsertTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void InsertTransaction(InsertTransactionRequest request, IUseCaseCallback<InsertTransactionResponse> callback)
        {
            int rowsModified = DBHandler.InsertTransaction(request.TransactionToInsert).Result;


            if (rowsModified > 0)
            {
                InsertTransactionResponse response = new InsertTransactionResponse();
                response.InsertedTransaction = request.TransactionToInsert;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankError error = new ZBankError();
                error.Type = ErrorType.UNKNOWN;
                callback.OnFailure(error);
            }
        }
    }
}
