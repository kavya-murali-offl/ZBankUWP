using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBank.Entity.BusinessObjects;
using ZBankManagement.Domain.UseCase;

namespace ZBank.DatabaseHandler
{
    public class DBHandler : IDBHandler
    {
        public DBHandler(IDatabaseAdapter databaseAdapter)
        {
            DatabaseAdapter = databaseAdapter;
            CreateTables();
        }
        private IDatabaseAdapter DatabaseAdapter { get; set; }

        // Rewritten

        // Account
        public async Task<IEnumerable<Account>> GetAllAccounts(string customerID)
        {
            List<Account> accountsList = new List<Account>();
            var currentAccount = await DatabaseAdapter.Query<CurrentAccount>($"Select * from Account Inner Join CurrentAccount on CurrentAccount.AccountNumber = Account.AccountNumber where UserID = ?", "1111");
            var savingsAccount = await DatabaseAdapter.Query<SavingsAccount>($"Select * from Account Inner Join SavingsAccount on SavingsAccount.AccountNumber = Account.AccountNumber where UserID = ?", "1111");
            var termDepositAccounts = await DatabaseAdapter.Query<TermDepositAccount>($"Select * from Account Inner Join TermDepositAccount on TermDepositAccount.AccountNumber = Account.AccountNumber where UserID = ?", customerID);
            accountsList.AddRange(currentAccount);
            accountsList.AddRange(savingsAccount);
            accountsList.AddRange(termDepositAccounts);
            return accountsList;
        }

        public async Task InsertAccount(Account account, Type dtoType=null)
        {
            try
            {

                await DatabaseAdapter.RunInTransaction(async() =>
                {
                   await DatabaseAdapter.Insert(account).ConfigureAwait(false);
                   await DatabaseAdapter.Insert(account as CurrentAccount, typeof(CurrentAccountDTO)).ConfigureAwait(false);
                }).ConfigureAwait(false);

            }    catch(Exception ex) { 
                ZBankError error = new ZBankError();
                error.Message = ex.Message; 
            }   
            
        }

        // Customer

        public Task<int> InsertCustomer(Customer customer, CustomerCredentials credentials)
        {

            //DatabaseAdapter.Insert(credentials);
            return DatabaseAdapter.Insert(customer);
        }

        public Task<int> UpdateCustomer(Customer customer) => DatabaseAdapter.Update(customer);

        public Task<List<Customer>> GetCustomer(string phoneNumber) => DatabaseAdapter.GetAll<Customer>().Where(customer => customer.Phone.Equals(phoneNumber)).ToListAsync();

        // Customer Credentials

        public async Task<CustomerCredentials> GetCredentials(string customerID) {
           return await DatabaseAdapter.GetScalar<CustomerCredentials>($"Select * from CustomerCredentials Where CustomerCredentials.CustomerID = ?", customerID);
        }

        public Task<int> InsertCredentials(CustomerCredentials customerCredentials) => DatabaseAdapter.Insert(customerCredentials);

        public Task<int> UpdateCredentials(CustomerCredentials customerCredentials) => DatabaseAdapter.Update(customerCredentials);



        public async Task<int> UpdateAccount<T>(T account)
        {
            await DatabaseAdapter.Update<T>(account);
            return DatabaseAdapter.Update(account as Account).Result;
        }

        //Card

        public Task<int> InsertCard(Card card) => DatabaseAdapter.Insert(card); 

        public Task<int> UpdateCard(Card card)  => DatabaseAdapter.Update(card) ;


        // Credit Card

        public async Task<IEnumerable<CreditCard>> GetCreditCardByCustomerID(string customerID) =>
             await DatabaseAdapter.Query<CreditCard>($"Select * from Card Inner Join CreditCard on Card.ID = CreditCard.ID where CustomerID = ?", customerID);


        // Transaction

        public async Task<IEnumerable<Transaction>> GetTransactionByAccountNumber(string accountNumber) => await DatabaseAdapter.GetAll<Transaction>().Where(x => x.OwnerAccount.Equals(accountNumber) || x.OtherAccount.Equals(accountNumber)).OrderByDescending(x => x.RecordedOn).ToListAsync();

        public async Task<IEnumerable<Transaction>> GetTransactionByCardNumber(string cardNumber) => await DatabaseAdapter.GetAll<Transaction>().Where(x => x.CardNumber == cardNumber).OrderByDescending(x => x.RecordedOn).ToListAsync();

        public Task<int> InsertTransaction(Transaction transaction) => DatabaseAdapter.Insert(transaction);

        // Create tables

        public async void CreateTables()
        {
            await DatabaseAdapter.CreateTable<Customer>();
            await DatabaseAdapter.CreateTable<CustomerCredentials>();
            await DatabaseAdapter.CreateTable<Card>();
            await DatabaseAdapter.CreateTable<Account>();
            await DatabaseAdapter.CreateTable<CurrentAccountDTO>();
            await DatabaseAdapter.CreateTable<SavingsAccountDTO>();
            await DatabaseAdapter.CreateTable<TermDepositAccountDTO>();
            await DatabaseAdapter.CreateTable<Transaction>();
            await DatabaseAdapter.CreateTable<CreditCardDTO>();
            await DatabaseAdapter.CreateTable<DebitCardDTO>();
        }
    }
}
