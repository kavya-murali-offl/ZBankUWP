using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBank.Entity;
using ZBank.Entities.BusinessObjects;

namespace ZBank.DatabaseHandler
 {
    public interface IDBHandler
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
        Task<IEnumerable<Account>> GetAllAccounts(string userID);

        Task<IEnumerable<Card>> GetAllCards(string customerID);


        Task InsertAccount(Account account);

        Task<int> UpdateAccount<T>(T account);



        // Card
        Task<int> InsertCard(Card card);

        Task<int> UpdateCard(Card card);

        Task<IEnumerable<DebitCard>> GetCardByAccountNumber(string accountNumber);

        // Transaction

        Task<IEnumerable<Transaction>> GetTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<Transaction>> GetLatestMonthTransactionByAccountNumber(string accountNumber);

        Task<IEnumerable<Transaction>> GetTransactionByCardNumber(string cardNumber);

        Task<int> InsertTransaction(Transaction transaction);

        // Beneficiary

        Task<IEnumerable<Beneficiary>> GetBeneficiaries(string customerID);

        Task<int> AddBeneficiary(Beneficiary beneficiary);

        Task<int> UpdateBeneficiary(Beneficiary beneficiaryToUpdate);

        // Reset password

        Task<int> ResetPassword(CustomerCredentials credentials);


    }
}
