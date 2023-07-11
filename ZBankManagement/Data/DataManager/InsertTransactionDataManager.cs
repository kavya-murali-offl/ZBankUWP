using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    class InsertTransactionDataManager : IInsertTransactionDataManager
    {
        public InsertTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task InsertTransaction(InsertTransactionRequest request, IUseCaseCallback<InsertTransactionResponse> callback)
        {
            int rowsModified = await DBHandler.InsertTransaction(request.TransactionToInsert).ConfigureAwait(false);

            if (rowsModified > 0)
            {
                InsertTransactionResponse response = new InsertTransactionResponse();
                response.InsertedTransaction = request.TransactionToInsert;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankException error = new ZBankException();
                error.Type = ErrorType.UNKNOWN;
                callback.OnFailure(error);
            }
        }
    }
}
