using ZBank.Entities;

namespace BankManagementDB.Interface
{
    public interface IUpdateAccountDataManager
    {
        bool UpdateAccount(Account updatedAccount);
    }
}
