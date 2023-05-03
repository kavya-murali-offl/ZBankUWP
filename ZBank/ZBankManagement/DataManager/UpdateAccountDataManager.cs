using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public  class UpdateAccountDataManager : IUpdateAccountDataManager
    {
        public UpdateAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }


        public bool UpdateAccount(Account updatedAccount) => DBHandler.UpdateAccount(updatedAccount).Result;
    }
}
