using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBank.Entity.BusinessObjects;

namespace ZBankManagement.Interface
{
    public interface IAccountFactory
    {
        Account GetAccountByType(AccountType accountType);
    }
}
