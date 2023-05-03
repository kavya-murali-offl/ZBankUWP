using ZBank.Entities.BusinessObjects;
using ZBank.DatabaseHandler;

namespace BankManagementDB.Data.DataManager
{
    public class InsertSavingsAccountDataManager : IInsertSavingsAccountDataManager
    {
        public InsertSavingsAccountDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool InsertSavingsAccount(SavingsAccountDTO account) => DBHandler.InsertSavingsAccount(account).Result;
    }
}
