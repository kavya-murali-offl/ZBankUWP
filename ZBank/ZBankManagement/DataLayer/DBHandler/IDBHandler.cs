using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZBank.DatabaseHandler
    {
    public interface IDBHandler
    {

        // Customer
        Task<int> InsertCustomer(Customer customer, CustomerCredentials credentials);

        Task<int> UpdateCustomer(Customer customer);

        Task<List<Customer>> GetCustomer(string phoneNumber);

        
        // Customer Credentials

        Task<CustomerCredentials> GetCredentials(string customerID);

        Task<int> UpdateCredentials(CustomerCredentials customerCredentials);

        // Account
        Task<IEnumerable<Account>> GetAllAccounts(string userID);


        Task InsertAccount(Account account, Type type=null);

        Task<int> UpdateAccount<T>(T account);



        // Card
        Task<int> InsertCard(Card card);

        Task<int> UpdateCard(Card card);


        // Transaction

        Task<IEnumerable<Transaction>> GetTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<Transaction>> GetTransactionByCardNumber(string cardNumber);

        Task<int> InsertTransaction(Transaction transaction);


        // Create Table

        void CreateTables();
    }
}
