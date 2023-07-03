using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBank.Entity;
using ZBank.Entity.BusinessObjects;
using Windows.Storage;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.DTOs;
using ZBank.Entities.EnumerationType;

namespace ZBank.DatabaseHandler
 {
    interface IDBHandler
    {

        //Branch 

        Task<List<Branch>> GetBranchDetails();

        Task<int> DeleteBeneficiary(Beneficiary beneficiary);

        Task<IEnumerable<Account>> GetIFSCCodeByAccountNumber(string accountNumber);
       
        Task<IEnumerable<ExternalAccount>> ValidateExternalAccount(string accountNumber, string IFSCCode);

        Task<IEnumerable<Branch>> GetBranchByIfscCode(string ifscCode);

        // Customer
        Task InsertCustomer(Customer customer, CustomerCredentials credentials);

        Task<int> UpdateCustomer(Customer customer);

        Task<List<Customer>> GetCustomer(string phoneNumber);
        Task<IEnumerable<TransactionBObj>> FetchAllTodayTransactions(string accountNumber, string customerID);


        // Customer Credentials

        Task<CustomerCredentials> GetCredentials(string customerID);

        Task<int> UpdateCredentials(CustomerCredentials customerCredentials);

        // Account
        Task<IEnumerable<AccountBObj>> GetAllAccounts(string userID);

        Task<IEnumerable<CardBObj>> GetAllCards(string customerID);


        Task UpdateAccount(TermDepositAccount account);

        Task InsertAccount(Account account, IEnumerable<KYCDocuments> documents);

        // Card
        Task InsertCreditCard(Card card, CreditCardDTO creditCardDTO);

        Task InsertDebitCard(Card card, DebitCardDTO debitCardDTO);

        Task<int> UpdateCard(Card card);

        Task<IEnumerable<CardBObj>> GetCardByAccountNumber(string accountNumber);
        
        Task<IEnumerable<CardBObj>> GetCardByCardNumber(string accountNumber);

        // Transaction

        Task<IEnumerable<TransactionBObj>> GetAllTransactionByAccountNumber(string accountNumber, string customerID);

        Task<IEnumerable<TransactionBObj>> GetLatestMonthTransactionByAccountNumber(string accountNumber, string customerID);

        Task<int> InsertTransaction(Transaction transaction);

        // Beneficiary

        Task<IEnumerable<BeneficiaryBObj>> GetBeneficiaries(string customerID);

        Task<int> AddBeneficiary(Beneficiary beneficiary);

        Task<int> UpdateBeneficiary(Beneficiary beneficiaryToUpdate);

        // Reset password

        Task<int> ResetPassword(CustomerCredentials credentials);

        Task<int> ResetPin(string CardNumber, string pin);

        Task InitiateTransactionInternal(Account account, Account beneficiaryAccount, Transaction transaction, TransactionMetaData transactionMetaData, TransactionMetaData otherTransactionMetaData);

        Task InitiateTransactionExternal(Transaction transaction, Account account, TransactionMetaData metaData);

        Task<Account> GetAccountByAccountNumber(string customerID, string accountNumber);

        Task CreateTables();

        Task PopulateData();

        Task CloseDeposit(TermDepositAccount termDepositAccount, Account repaymentAccount, Transaction transaction);
    }
}
