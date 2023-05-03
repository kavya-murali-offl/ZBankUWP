using BankManagementDB.Interface;
using ZBank.DatabaseHandler;
using ZBank.Entities;

namespace BankManagementDB.DataManager
{
    public class InsertAccountDataManager : IInsertAccountDataManager
    {
        public InsertAccountDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool InsertAccount(Account account)
        {

            return DBHandler.InsertAccount(account).Result;
            //Store.AccountsList ??= new List<Account>();

            //if (isSuccess)
            //{
            //    if (account is CurrentAccount)
            //    {
            //        CurrentAccountDTO currentAccount = new CurrentAccountDTO();
            //        currentAccount.ID = account.ID;
            //        DBHandler.InsertCurrentAccount(currentAccount);
            //    }
            //    else if (account is SavingsAccount)
            //    {
            //        SavingsAccountDTO savingsAccountDTO = new SavingsAccountDTO();
            //        savingsAccountDTO.ID = account.ID;
            //        DBHandler.InsertSavingsAccount(savingsAccountDTO);
            //    }
            //    Store.AccountsList.Prepend(account);
            //}
            //return isSuccess;
        }
    }
}
