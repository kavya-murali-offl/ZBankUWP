using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBankManagement.Domain.UseCase;
using System.Threading;
using ZBankManagement.Controller;
using System.Security.Principal;
using ZBank.Entity;
using System.Linq;
using System.Collections.ObjectModel;
using ZBank.Entity.BusinessObjects;

namespace ZBank.DatabaseHandler
{
    public class DBHandler : IDBHandler
    {
        public DBHandler(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }
        private IDatabaseAdapter _databaseAdapter { get; set; }

        // Rewritten

        public async Task<IEnumerable<CardBObj>> GetAllCards(string customerID)
        {
            List<CardBObj> cardsList = new List<CardBObj>();
            var creditCards = await _databaseAdapter.Query<CreditCard>($"Select * from Card" +
                $" Inner Join CreditCard on CreditCard.CardNumber = Card.CardNumber " +
                $"where CustomerID = ?", "1111");
            var debitCards = await _databaseAdapter.Query<DebitCard>($"Select * from Card " +
                $"Inner Join DebitCard on DebitCard.CardNumber = Card.CardNumber " +
                $"where CustomerID = ?", "1111");
            cardsList.AddRange(creditCards);
            cardsList.AddRange(debitCards);
            return cardsList;
        }

        public Task<List<Branch>> GetBranchDetails()
        {
            return _databaseAdapter.GetAll<Branch>().ToListAsync();
        }

        public Task<IEnumerable<Branch>> GetBranchByIfscCode(string ifscCode)
        {
            return _databaseAdapter.Query<Branch>("Select * from Branch where IfscCode = ?", ifscCode);
        }


        public Task<IEnumerable<CardBObj>> GetCardByAccountNumber(string accountNumber)
        {
            return _databaseAdapter.Query<CardBObj>("Select * from Card Inner Join DebitCard on DebitCard.CardNumber = Card.CardNumber where AccountNumber = ?", accountNumber);
        }

        public async Task<IEnumerable<CardBObj>> GetCardByCardNumber(string cardNumber)
        {
            List<CardBObj> cardsList = new List<CardBObj>();
            var debitCard = await _databaseAdapter.Query<DebitCard>("Select * from Card Inner Join DebitCard on DebitCard.CardNumber = Card.CardNumber where Card.CardNumber = ?", cardNumber);
            var creditCard = await _databaseAdapter.Query<CreditCard>("Select * from Card Inner Join CreditCard on CreditCard.CardNumber = Card.CardNumber where Card.CardNumber = ?", cardNumber);
            cardsList.AddRange(debitCard);
            cardsList.AddRange(creditCard);
            return cardsList;
        }

        // Account
        public async Task<IEnumerable<AccountBObj>> GetAllAccounts(string customerID)
        {
            List<AccountBObj> accountsList = new List<AccountBObj>();

            var currentAccount = await _databaseAdapter.Query<CurrentAccount>($"Select * from Account " +
                $"Inner Join CurrentAccount on CurrentAccount.AccountNumber = Account.AccountNumber " +
                $"Inner Join Branch on Branch.IfscCode = Account.IFSCCode " +
                $"Left Join DebitCard on DebitCard.AccountNumber = Account.AccountNumber " +
                $"where IsKYCApproved and UserID = ?", "1111");
            var savingsAccount = await _databaseAdapter.Query<SavingsAccount>($"Select * from Account " +
                $"Inner Join SavingsAccount on SavingsAccount.AccountNumber = Account.AccountNumber " +
                $"Inner Join Branch on Branch.IfscCode = Account.IFSCCode " +
                $"Left Join DebitCard on DebitCard.AccountNumber = Account.AccountNumber " +
                $"where IsKYCApproved and UserID = ?", "1111");
            var termDepositAccounts = await _databaseAdapter.Query<TermDepositAccount>($"Select * from Account " +
                $"Inner Join TermDepositAccount on TermDepositAccount.AccountNumber = Account.AccountNumber " +
                $"Left Join DebitCard on DebitCard.AccountNumber = Account.AccountNumber " +
                $"Inner Join Branch on Branch.IfscCode = Account.IFSCCode " +
                $"where IsKYCApproved and UserID = ?", "1111");

            accountsList.AddRange(currentAccount);
            accountsList.AddRange(savingsAccount);
            accountsList.AddRange(termDepositAccounts);
            return accountsList;
        }

        public async Task<bool> InsertAccount(Account account)
        {
            int rowsModified = 0;
            object dtoObject = AccountFactory.GetDTOObject(account);
            rowsModified += await _databaseAdapter.Insert(account, typeof(Account));
            rowsModified += await _databaseAdapter.Insert(dtoObject);
            return rowsModified == 2;
        }

        // Beneficiaries

        public async Task<IEnumerable<Beneficiary>> GetBeneficiaries(string customerID)
        {
            return await _databaseAdapter.Query<Beneficiary>($"Select * from Beneficiary where UserID = ?", "1111");
        }

        public Task<int> AddBeneficiary(Beneficiary beneficiary) => _databaseAdapter.Update(beneficiary);

        public Task<int> UpdateBeneficiary(Beneficiary beneficiaryToUpdate) => _databaseAdapter.Update(beneficiaryToUpdate);

        // Reset password

        public Task<int> ResetPassword(CustomerCredentials credentials) => _databaseAdapter.Update(credentials);


        // =========================================== \\ // ========================================== \\
        // Customer

        public Task<int> InsertCustomer(Customer customer, CustomerCredentials credentials)
        {
            return null;
        }

        public Task<int> UpdateCustomer(Customer customer) => _databaseAdapter.Update(customer);

        public Task<List<Customer>> GetCustomer(string phoneNumber) => _databaseAdapter.GetAll<Customer>().Where(customer => customer.Phone.Equals(phoneNumber)).ToListAsync();


        // Customer Credentials

        public async Task<CustomerCredentials> GetCredentials(string customerID)
        {
            return await _databaseAdapter.GetScalar<CustomerCredentials>($"Select * from CustomerCredentials Where CustomerCredentials.CustomerID = ?", customerID);
        }

        public Task<int> InsertCredentials(CustomerCredentials customerCredentials) => _databaseAdapter.Insert(customerCredentials);

        public Task<int> UpdateCredentials(CustomerCredentials customerCredentials) => _databaseAdapter.Update(customerCredentials);



        public async Task<int> UpdateAccount<T>(T account)
        {
            await _databaseAdapter.Update<T>(account);
            return _databaseAdapter.Update(account as Entities.Account).Result;
        }

        //Card

        public Task<int> InsertCard(Card card) => _databaseAdapter.Insert(card);

        public Task<int> UpdateCard(Card card) => _databaseAdapter.Update(card);


        // Credit Card

        public async Task<IEnumerable<CreditCard>> GetCreditCardByCustomerID(string customerID) =>
             await _databaseAdapter.Query<CreditCard>($"Select * from Card Inner Join CreditCard on Card.ID = CreditCard.ID where CustomerID = ?", customerID);


        // Transaction

        public async Task<IEnumerable<TransactionBObj>> GetTransactionByAccountNumber(string accountNumber)
        {
          var transactionBObjs =  await _databaseAdapter.Query<TransactionBObj>(
              $"Select * from Transactions Left Join Beneficiary on Transactions.OtherAccountNumber = Beneficiary.AccountNumber where OwnerAccountNumber = ?", accountNumber);
            return transactionBObjs;
        }
        //await
        //_databaseAdapter.GetAll<TransactionBObj>()
        //.Where(x => x.OwnerAccountNumber.Equals(accountNumber)
        //.OrderByDescending(x => x.RecordedOn)
        //.ToListAsync();

        public async Task<IEnumerable<TransactionBObj>> GetTransactionByCardNumber(string cardNumber) =>
            await _databaseAdapter.GetAll<TransactionBObj>()
            .Where(x => x.CardNumber == cardNumber)
            .OrderByDescending(x => x.RecordedOn)
            .ToListAsync();

        public Task<int> InsertTransaction(Transaction transaction) => _databaseAdapter.Insert(transaction);

        public async Task<IEnumerable<TransactionBObj>> GetLatestMonthTransactionByAccountNumber(string accountNumber)
        {
            return await _databaseAdapter.Query<TransactionBObj>("SELECT * FROM Transactions " +
                $"Inner Join Beneficiary on Transactions.OtherAccountNumber = Beneficiary.AccountNumber " +
                "WHERE OwnerAccountNumber == ? AND RecordedOn < date('now','-30 days')", accountNumber);
        }

        public async Task<IEnumerable<TransactionBObj>> GetAllTransactionByAccountNumber(string accountNumber)
        {
            return await _databaseAdapter.Query<TransactionBObj>("SELECT * FROM Transactions " +
                $"Inner Join Beneficiary on Transactions.OtherAccountNumber = Beneficiary.AccountNumber " +
                "WHERE OwnerAccountNumber == ?", accountNumber);
        }

    }
}
