using ZBank.DatabaseHandler;
using ZBank.Entities;

namespace BankManagementDB.DataManager
{
    public class InsertCardDataManager : IInsertCardDataManager
    {
        public InsertCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool InsertCard(Card card) => DBHandler.InsertCard(card).Result;
    }
}
