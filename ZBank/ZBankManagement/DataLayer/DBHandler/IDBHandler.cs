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
        Task<bool> InsertCustomer(Customer customer, CustomerCredentials credentials);

        Task<bool> UpdateCustomer(Customer customer);

        Task<List<Customer>> GetCustomer(string phoneNumber);

        
        // Customer Credentials

        Task<CustomerCredentials> GetCredentials(string customerID);

        Task<bool> UpdateCredentials(CustomerCredentials customerCredentials);

        // Account
        Task<IEnumerable<Account>> GetAllAccounts(string userID);


        Task InsertAccount<T>(T dtoAccount, Account account);

        Task<bool> UpdateAccount<T>(T account);


        // Debit Card
        Task<IEnumerable<DebitCard>> GetDebitCardByCustomerID(string customerID);

        Task<bool> InsertDebitCard(DebitCardDTO card);

        Task<bool> UpdateDebitCard(DebitCardDTO card);



        // Card
        Task<bool> InsertCard(Card card);

        Task<bool> UpdateCard(Card card);


        // Transaction

        Task<IEnumerable<Transaction>> GetTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<Transaction>> GetTransactionByCardNumber(string cardNumber);

        Task<bool> InsertTransaction(Transaction transaction);


        // Create Table

        void CreateTables();
    }
}
