using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertTransaction;
using BankManagementDB.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;

namespace BankManagementDB.DataManager
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
            bool success = DBHandler.InsertTransaction(request.TransactionToInsert).Result;

            InsertTransactionResponse response = new InsertTransactionResponse();
            response.IsSuccess = success;   

            if (success)
            {
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
