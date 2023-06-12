using ZBankManagement.Domain.UseCase;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    public class InsertCardDataManager : IInsertCardDataManager
    {
        public InsertCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback)
        {
            int rowsModified = await DBHandler.InsertCard(request.CardToInsert);
        }
    }
}
