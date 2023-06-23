using BankManagementDB.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using Windows.Storage;
using ZBank.DatabaseAdapter;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entity;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBankManagement.Controller;
using ZBankManagement.Entity.DTOs;

namespace ZBank.DatabaseHandler
{
    class DBHandler : IDBHandler
    {
        public DBHandler(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        public async Task InsertAccount(Account account, IReadOnlyList<StorageFile> documents)
        {
            object dtoObject = AccountFactory.GetDTOObject(account);
            Func<Task> func = async () =>
            {
                await _databaseAdapter.Insert(account, typeof(Account));
                await _databaseAdapter.Insert(dtoObject);
                foreach(var file in documents)
                {
                    byte[] fileBytes = await GetBytesFromFile(file);
                    KYCDocuments kycDoc = new KYCDocuments()
                    {
                        ID = Guid.NewGuid().ToString(),
                        File = fileBytes,
                        FileName = file.Name,
                        UploadedOn = DateTime.Now,

                    };
                    await _databaseAdapter.Insert(kycDoc);
                }
            };
            await _databaseAdapter.RunInTransactionAsync(func);
        }
        public async Task<byte[]> GetBytesFromFile(StorageFile file)
        {
            var stream = await file.OpenStreamForReadAsync();
            byte[] bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        private IDatabaseAdapter _databaseAdapter { get; set; }

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
        public async Task PopulateData()
        {
            await _databaseAdapter.Insert(new Bank()
            {
                ID = 1,
                Name = "ZBank"
            });

            List<CurrentAccountDTO> currentAccountDTOs = new List<CurrentAccountDTO>()
            {
                new CurrentAccountDTO()
                {
                    AccountNumber="1000 1789 7890 6633",
                     InterestRate=3.4m,
                     MinimumBalance=500,
                }
            };

            await _databaseAdapter.InsertAll(currentAccountDTOs);

            List<SavingsAccountDTO> savingsAccountDTOs = new List<SavingsAccountDTO>()
            {
                new SavingsAccountDTO()
                {
                    AccountNumber="1001 1000 1789 7890",
                    InterestRate=5.4m,
                }
            };

            await _databaseAdapter.InsertAll(savingsAccountDTOs);

            List<TermDepositAccountDTO> termDepositAccountDTOs = new List<TermDepositAccountDTO>()
            {
                new TermDepositAccountDTO()
                {
                    AccountNumber="1009 6678 5556 3332",
                    DepositStartDate=DateTime.Now,
                 InterestRate=7.2m,
                  DepositType=DepositType.OnMaturity,
                   MaturityAmount=200000,
                    MaturityDate=DateTime.Now.AddYears(1),
              RepaymentAccountNumber="1000 1789 7890 6633",
               Tenure=12
                }
            };

            await _databaseAdapter.InsertAll(termDepositAccountDTOs);

            List<Account> accounts = new List<Account>
                {
                new Account()
                {
                    AccountNumber = "1000 1789 7890 6633",
                    IFSCCode = "ZBNK1001",
                    AccountName = "Kavya",
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                    AccountStatus = AccountStatus.ACTIVE,
                    Currency = Currency.INR,
                    Balance = 10000m,
                    AccountType = AccountType.CURRENT,
                    IsKYCApproved = true
                },

                new Account()
                {
                    AccountNumber = "1001 1000 1789 7890",
                    IFSCCode = "ZBNK1002",
                    AccountName = "Kavya",
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                    AccountStatus = AccountStatus.ACTIVE,
                    Currency = Currency.INR,
                    Balance = 0m,
                    AccountType = AccountType.SAVINGS,
                    IsKYCApproved = true
                },

                  new Account()
                {
                    AccountNumber = "1009 6678 5556 3332",
                    IFSCCode = "ZBNK1001",
                    AccountName = "Kavya",
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                    AccountStatus = AccountStatus.ACTIVE,
                    Currency = Currency.INR,
                    Balance = 0m,
                    IsKYCApproved = true,
                    AccountType = AccountType.TERM_DEPOSIT,
                },
            };

            await _databaseAdapter.InsertAll(accounts);


            List<Beneficiary> beneficiaries = new List<Beneficiary>()
            {
                new Beneficiary()
                {
                    ID="1",
                    AccountNumber="1009 6678 5556 3332",
                     IFSCCode="HDFC1001",
                      Name="John",
                      ProfilePicture="",
                       UserID="1111"
                },
                new Beneficiary()
                {
                    ID="2",
                    AccountNumber="1234 7654 9876 9874",
                     IFSCCode="HDFC1002",
                      Name="Preethi",
                      ProfilePicture="",
                       UserID="1111"
                },
            };
            await _databaseAdapter.InsertAll(beneficiaries);

            List<Branch> branches = new List<Branch>()
            {
                new Branch()
                {
                    BankID = "1",
                    BranchID = "1",
                     BranchName="POJN",
                     IfscCode="ZBNK1001"
                },
                new Branch()
                {
                    BankID = "1",
                    BranchID = "2",
                     BranchName="POIU",
                     IfscCode="ZBNK1002"
                },
                new Branch()
                {
                    BankID = "2",
                    BranchID = "3",
                     BranchName="RYUI",
                     IfscCode="HDFC1001"
                },
            };
            await _databaseAdapter.InsertAll(branches);

            List<Card> cards = new List<Card>()
            {
                new Card()
                {
                     CardNumber="1245 6888 0087 56788",
                      CustomerID="1111",
                       CVV="122",
                         ExpiryMonth="12",
                          ExpiryYear="2027",
                           LinkedOn=DateTime.Now,
                            Pin="2001",
                            Type=Entities.CardType.DEBIT
                },

                 new Card()
                {
                     CardNumber="7678 6667 0087 56788",
                      CustomerID="1111",
                       CVV="122",
                         ExpiryMonth="12",
                          ExpiryYear="2027",
                           LinkedOn=DateTime.Now,
                            Pin="1002",
                            Type=Entities.CardType.CREDIT
                },
            };

           await _databaseAdapter.InsertAll(cards);

            List<CreditCardDTO> creditCardDTOs = new List<CreditCardDTO>()
            {
                new CreditCardDTO()
                {
                     CardNumber="7678 6667 0087 56788",
                     CreditLimit=30000m,
                     Interest=2.3m,
                     MinimumOutstanding=2000m,
                     Provider=CreditCardProvider.MASTERCARD,
                     TotalOutstanding=10000m
                }
            };

            await _databaseAdapter.InsertAll(creditCardDTOs);

            List<DebitCardDTO> debitCardDTOs = new List<DebitCardDTO>()
            {
                new DebitCardDTO()
                {
                    CardNumber="1245 6888 0087 56788",
                    AccountNumber="1000 1789 7890 6633",
                }
            };

            await _databaseAdapter.InsertAll(debitCardDTOs);

            List<Customer> customers = new List<Customer>()
            {
                new Customer()
                {
                     Age=23,
                      CreatedOn=DateTime.Now,
                       Email="kav@gmail.com",
                        ID="1111",
                         LastLoggedOn=DateTime.Now,
                         Name="Kavya",
                          Phone="1234567890"
                }
            };

            await _databaseAdapter.InsertAll(customers);

            List<CustomerCredentials> customerCredentials = new List<CustomerCredentials>()
            {
                new CustomerCredentials()
                {
                    ID="1111",
                    Password="1122",
                    Salt="1111"
                }
            };

            _databaseAdapter.InsertAll(customerCredentials);


            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                     ReferenceID = "123456uhgfuy65",
                      Amount=12000m,
                       Balance=12000m,
                        CardNumber="",
                         Description="Salary",
                          ModeOfPayment=Entities.ModeOfPayment.DIRECT,
                          ToAccountNumber="1009 6678 5556 3332",
                          FromAccountNumber="1000 1789 7890 6633",
                           RecordedOn=DateTime.Now,
                },

                new Transaction()
                {
                     ReferenceID = "dfgyhu5678905u",
                      Amount=2000m,
                       Balance=10000m,
                        CardNumber="",
                         Description="Payment",
                          ModeOfPayment=Entities.ModeOfPayment.DIRECT,
                          ToAccountNumber="1234 7654 9876 9874",
                          FromAccountNumber="1000 1789 7890 6633",
                           RecordedOn=DateTime.Now,
                }
                };

            await _databaseAdapter.InsertAll(transactions);

        }


        public Task<List<Branch>> GetBranchDetails()
        {
            return _databaseAdapter.GetAll<Branch>().ToListAsync();
        }

        public async Task InitiateTransactionExternal(Transaction transaction, Account ownerAccount)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Update(ownerAccount);
                await _databaseAdapter.Insert(transaction);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

        public async Task<Account> GetAccountByAccountNumber(string accountNumber)
        {
            return await _databaseAdapter.GetAll<Account>().Where(acc => acc.AccountNumber == accountNumber).FirstOrDefaultAsync();    
        }


        public async Task InitiateTransactionInternal(Transaction transaction, Account ownerAccount, Account beneficiaryAccount)
        {

            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(transaction);
                await _databaseAdapter.Update(ownerAccount);
                await _databaseAdapter.Update(beneficiaryAccount);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
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

        public async Task CreateTables()
        {
           await _databaseAdapter.CreateTable<Customer>();
            await _databaseAdapter.CreateTable<CustomerCredentials>();
            await _databaseAdapter.CreateTable<Card>();
            await _databaseAdapter.CreateTable<Account>();
            await _databaseAdapter.CreateTable<Beneficiary>();
            await _databaseAdapter.CreateTable<CurrentAccountDTO>();
            await _databaseAdapter.CreateTable<SavingsAccountDTO>();
            await _databaseAdapter.CreateTable<TermDepositAccountDTO>();
            await _databaseAdapter.CreateTable<Transaction>();
            await  _databaseAdapter.CreateTable<CreditCardDTO>();
            await _databaseAdapter.CreateTable<DebitCardDTO>();
           await _databaseAdapter.CreateTable<Bank>();
            await _databaseAdapter.CreateTable<Branch>();
            await _databaseAdapter.CreateTable<KYCDocuments>();
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
            return await _databaseAdapter.Update(account as Entities.Account);
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
            var transactionBObjs = await _databaseAdapter.Query<TransactionBObj>(
                $"Select * from Transactions Left Join Beneficiary on Transactions.ToAccountNumber = Beneficiary.AccountNumber where FromAccountNumber = ?", accountNumber);
            return transactionBObjs;
        }
       

        public async Task<IEnumerable<TransactionBObj>> GetTransactionByCardNumber(string cardNumber) =>
            await _databaseAdapter.GetAll<TransactionBObj>()
            .Where(x => x.CardNumber == cardNumber)
            .OrderByDescending(x => x.RecordedOn)
            .ToListAsync();

        public Task<int> InsertTransaction(Transaction transaction) => _databaseAdapter.Insert(transaction);

        public async Task<IEnumerable<TransactionBObj>> GetLatestMonthTransactionByAccountNumber(string accountNumber)
        {
            return await _databaseAdapter.Query<TransactionBObj>("SELECT * FROM Transactions " +
                $"Inner Join Beneficiary on Transactions.ToAccountNumber = Beneficiary.AccountNumber " +
                "WHERE FromAccountNumber == ? AND RecordedOn < date('now','-30 days')", accountNumber);
        }

        public async Task<IEnumerable<TransactionBObj>> GetAllTransactionByAccountNumber(string accountNumber)
        {
            return await _databaseAdapter.Query<TransactionBObj>("SELECT * FROM Transactions " +
                $"Inner Join Beneficiary on Transactions.ToAccountNumber = Beneficiary.AccountNumber " +
                "WHERE FromAccountNumber == ?", accountNumber);
        }

    }
}
