using ZBank.Entities.BusinessObjects;

namespace BankManagementDB.Interface
{
    public interface IInsertDebitCardDataManager
    {
        bool InsertDebitCard(DebitCardDTO card);
    }
}
