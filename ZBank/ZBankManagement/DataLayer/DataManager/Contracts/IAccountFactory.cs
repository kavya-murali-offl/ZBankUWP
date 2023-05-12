using ZBank.Entities.EnumerationType;
using ZBank.Entities;

namespace ZBankManagement.Interface
{
    public interface IAccountFactory
    {
        Account GetAccountByType(AccountType accountType);
    }
}
