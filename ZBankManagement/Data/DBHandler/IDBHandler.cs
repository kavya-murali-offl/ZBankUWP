using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBank.Entity;
using ZBank.Entity.BusinessObjects;
using Windows.Storage;

namespace ZBank.DatabaseHandler
 {
    interface IDBHandler
    {

        //Branch 

        Task<List<Branch>> GetBranchDetails();

        Task<IEnumerable<Branch>> GetBranchByIfscCode(string ifscCode);

        // Customer
        Task<int> InsertCustomer(Customer customer, CustomerCredentials credentials);

        Task<int> UpdateCustomer(Customer customer);

        Task<List<Customer>> GetCustomer(string phoneNumber);

        
        // Customer Credentials

        Task<CustomerCredentials> GetCredentials(string customerID);

        Task<int> UpdateCredentials(CustomerCredentials customerCredentials);

        // Account
        Task<IEnumerable<AccountBObj>> GetAllAccounts(string userID);

        Task<IEnumerable<CardBObj>> GetAllCards(string customerID);


        Task<int> UpdateAccount<T>(T account);

        Task InsertAccount(Account account, IReadOnlyList<StorageFile> storageFiles);

        // Card
        Task<int> InsertCard(Card card);

        Task<int> UpdateCard(Card card);

        Task<IEnumerable<CardBObj>> GetCardByAccountNumber(string accountNumber);
        
        Task<IEnumerable<CardBObj>> GetCardByCardNumber(string accountNumber);

        // Transaction

        Task<IEnumerable<TransactionBObj>> GetTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<TransactionBObj>> GetAllTransactionByAccountNumber(string accountNumber);


        Task<IEnumerable<TransactionBObj>> GetLatestMonthTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<TransactionBObj>> GetTransactionByCardNumber(string cardNumber);

        Task<int> InsertTransaction(Transaction transaction);

        // Beneficiary

        Task<IEnumerable<Beneficiary>> GetBeneficiaries(string customerID);

        Task<int> AddBeneficiary(Beneficiary beneficiary);

        Task<int> UpdateBeneficiary(Beneficiary beneficiaryToUpdate);

        // Reset password

        Task<int> ResetPassword(CustomerCredentials credentials);
    }
}
