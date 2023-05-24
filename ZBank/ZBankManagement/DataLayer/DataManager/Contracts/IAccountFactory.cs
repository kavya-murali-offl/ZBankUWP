using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;

namespace ZBankManagement.Interface
{
    public interface IAccountFactory
    {
        Account GetAccountByType(AccountType accountType);
    }
}
