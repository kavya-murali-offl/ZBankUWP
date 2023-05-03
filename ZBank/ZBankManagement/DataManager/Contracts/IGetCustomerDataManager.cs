using ZBank.Entities;

namespace BankManagementDB.Interface
{
    public interface IGetCustomerDataManager
    {
        Customer GetCustomer(string phoneNumber);

    }
}
