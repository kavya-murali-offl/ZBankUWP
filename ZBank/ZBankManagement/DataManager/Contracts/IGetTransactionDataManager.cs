using ZBank.Entities;
using System.Collections.Generic;

namespace BankManagementDB.Interface
{
    public interface IGetTransactionDataManager
    {
        IEnumerable<Transaction> GetTransactionsByCardNumber(string cardNumber);

        IEnumerable<Transaction> GetTransactionsByAccountNumber(string accountID);

    }
}
