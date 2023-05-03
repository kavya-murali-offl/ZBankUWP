using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class InsertCredentialsDataManager : IInsertCredentialsDataManager
    {

        public InsertCredentialsDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool InsertCredentials(CustomerCredentials customerCredentials) => DBHandler.InsertCredentials(customerCredentials).Result;

    }
}
