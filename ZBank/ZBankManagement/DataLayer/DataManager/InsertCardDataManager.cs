using BankManagementDB.Domain.UseCase;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertCard;

namespace BankManagementDB.DataManager
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
            bool result = DBHandler.InsertCard(request.CardToInsert).Result;
        }
    }
}
