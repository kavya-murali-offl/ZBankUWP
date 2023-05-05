using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class UpdateCustomerDataManager : IUpdateCustomerDataManager
    {
        public UpdateCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool UpdateCustomer(Customer customer)
        {
            bool success = DBHandler.UpdateCustomer(customer).Result;
            return success;
        }
    }
}
