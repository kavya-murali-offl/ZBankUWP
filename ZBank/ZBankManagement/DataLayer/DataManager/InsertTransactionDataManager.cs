using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class InsertTransactionDataManager : IInsertTransactionDataManager
    {
        public InsertTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool InsertTransaction(Transaction transaction)
        {
            bool success = DBHandler.InsertTransaction(transaction).Result;
            if (success)
            {
                //Store.TransactionsList ??= new List<Transaction>();
                //Store.TransactionsList.Prepend(transaction);
            }
            return success;
        }
    }
}
