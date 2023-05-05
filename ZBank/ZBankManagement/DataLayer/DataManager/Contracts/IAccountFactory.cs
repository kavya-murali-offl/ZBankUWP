using ZBank.Entities.EnumerationType;
using ZBank.Entities;

namespace BankManagementDB.Interface
{
    public interface IAccountFactory
    {
        Account GetAccountByType(AccountType accountType);
    }
}
