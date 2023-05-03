using ZBank.Entities;

namespace BankManagementDB.Interface
{
    public interface IInsertTransactionDataManager
    {
        bool InsertTransaction(Transaction transaction);
    }
}
