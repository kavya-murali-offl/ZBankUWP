using BankManagementDB.Interface;
using ZBank.Entities;
using System.Linq;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class GetCustomerDataManager  : IGetCustomerDataManager
    {
        public GetCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }
        
        public Customer GetCustomer(string phoneNumber) => DBHandler.GetCustomer(phoneNumber).Result.FirstOrDefault();
    }
}
