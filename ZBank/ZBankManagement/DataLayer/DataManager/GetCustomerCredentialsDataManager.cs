using BankManagementDB.Interface;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class GetCustomerCredentialsDataManager 
    {
        public GetCustomerCredentialsDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

      
    }
}
