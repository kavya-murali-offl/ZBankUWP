using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class UpdateCardDataManager : IUpdateCardDataManager
    {

        public UpdateCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool UpdateCard(Card updatedCard) =>
             DBHandler.UpdateCard(updatedCard).Result;

    }
}
