using BankManagementDB.Data;
using BankManagementDB.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class GetTransactionDataManager : IGetTransactionDataManager
    {
        public GetTransactionDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public IEnumerable<Transaction> GetTransactionsByAccountNumber(string accountID)
        {
            Store.TransactionsList = DBHandler.GetTransactionByAccountNumber(accountID).Result;
            return Store.TransactionsList;
        }

        public IEnumerable<Transaction> GetTransactionsByCardNumber(string cardNumber) => DBHandler.GetTransactionByCardNumber(cardNumber).Result;
    }
}
