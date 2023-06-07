using ZBankManagement.Domain.UseCase;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public class InsertCardDataManager : IInsertCardDataManager
    {
        public InsertCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback)
        {
            int rowsModified = DBHandler.InsertCard(request.CardToInsert).Result;
        }
    }
}
