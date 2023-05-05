using ZBank.Entities;
using System;
using System.Collections.Generic;

namespace BankManagementDB.Interface
{
    public interface IAccountDataManager
    {
        bool InsertAccount(Account account);

        bool UpdateAccount(Account account);

        void GetAllAccounts(Guid id);

        IList<Account> GetAccountsList();

        bool UpdateBalance(Account account, decimal amount, ZBank.Entities.EnumerationType.TransactionType transactionType);

        Account GetAccountByQuery(string key, string value);
    }
}
