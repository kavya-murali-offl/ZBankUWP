using BankManagementDB.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Provider;
using ZBank.DatabaseAdapter;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entity;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBankManagement.Controller;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Entity.DTOs;
using ZBankManagement.Entity.EnumerationTypes;

namespace ZBank.DatabaseHandler
{
    class DBHandler : IDBHandler
    {
        public DBHandler(IDatabaseAdapter databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        public Task<IEnumerable<Account>> GetIFSCCodeByAccountNumber(string accountNumber)
        {
            return _databaseAdapter.Query<Account>("Select IFSCCode From Account where AccountNumber = ?", accountNumber);
        }

        public async Task<IEnumerable<ExternalAccount>> ValidateExternalAccount(string accountNumber, string IFSCCode)
        {
            return await _databaseAdapter.Query<ExternalAccount>("Select * from ExternalAccounts where ExternalAccountNumber = ? and ExternalIFSCCode = ?", accountNumber, IFSCCode);
        }


        public async Task InsertAccount(Account account, IEnumerable<KYCDocuments> documents)
        {
            object dtoObject = AccountFactory.GetDTOObject(account);
            Func<Task> func = async () =>
            {
                await _databaseAdapter.Insert(account, typeof(Account));
                await _databaseAdapter.Insert(dtoObject);
                foreach (var kycDoc in documents)
                {

                    await _databaseAdapter.Insert(kycDoc);
                }
            };
            await _databaseAdapter.RunInTransactionAsync(func);
        }



        private IDatabaseAdapter _databaseAdapter { get; set; }

        public async Task<IEnumerable<CardBObj>> GetAllCards(string customerID)
        {
            List<CardBObj> cardsList = new List<CardBObj>();
            var creditCards = await _databaseAdapter.Query<CreditCard>($"Select * from Card" +
                $" Inner Join CreditCard on CreditCard.CardNumber = Card.CardNumber " +
                $"where CustomerID = ?", customerID);
            var debitCards = await _databaseAdapter.Query<DebitCard>($"Select * from Card " +
                $"Inner Join DebitCard on DebitCard.CardNumber = Card.CardNumber " +
                $"where CustomerID = ?", customerID);
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
                },
                 new CurrentAccountDTO()
                {
                    AccountNumber="1666 5788 4567 5676",
                     InterestRate=3.4m,
                     MinimumBalance=500,
                },

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
                    UserID = "80600504",
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
                    UserID = "80600504",
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
                    UserID = "80600504",
                    CreatedOn = DateTime.Now,
                    AccountStatus = AccountStatus.ACTIVE,
                    Currency = Currency.INR,
                    Balance = 0m,
                    IsKYCApproved = true,
                    AccountType = AccountType.TERM_DEPOSIT,
                },
                  new Account()
                  {
                    AccountNumber="1666 5788 4567 5676",
                     IFSCCode = "ZBNK1002",
                      AccountName = "John",
                    UserID = "2222",
                    CreatedOn = DateTime.Now,
                    AccountStatus = AccountStatus.ACTIVE,
                    Currency = Currency.INR,
                    Balance = 0m,
                    IsKYCApproved = true,
                    AccountType = AccountType.CURRENT,
                  }
            };

            await _databaseAdapter.InsertAll(accounts);


            List<Beneficiary> beneficiaries = new List<Beneficiary>()
            {
                new Beneficiary()
                {
                    ID="1",
                    BeneficiaryName="John",
                    AccountNumber="1666 5788 4567 5676",
                    BeneficiaryType = BeneficiaryType.WITHIN_BANK,
                    UserID="80600504"
                },
                new Beneficiary()
                {
                    ID="2",
                    BeneficiaryName="Preethi",
                    AccountNumber="1234 7654 9876 9874",
                    BeneficiaryType = BeneficiaryType.OTHER_BANK,
                    UserID="80600504"
                },
            };
            await _databaseAdapter.InsertAll(beneficiaries);
            var id1 = Guid.NewGuid().ToString();
            var id2 = Guid.NewGuid().ToString();
            var id3 = Guid.NewGuid().ToString();
            List<Transaction> transactions = new List<Transaction>()
            {
                new Transaction()
                {
                     ReferenceID = id1,
                     TransactionType = Entities.TransactionType.INTERNAL,
                      Amount=12000m,
                         Description="Salary",
                          SenderAccountNumber="1666 5788 4567 5676",
                          RecipientAccountNumber="1000 1789 7890 6633",
                           RecordedOn=DateTime.Now,
                },

                new Transaction()
                {
                     ReferenceID = id2,
                      Amount=2000m,
                     TransactionType = Entities.TransactionType.EXTERNAL,
                         Description="Payment",
                         SenderAccountNumber ="1234 7654 9876 9874",
                          RecipientAccountNumber="1000 1789 7890 6633",
                           RecordedOn=DateTime.Now,
                },

                 new Transaction()
                {
                     ReferenceID = id3,
                      Amount=5000m,
                     TransactionType = Entities.TransactionType.EXTERNAL,
                         Description="Payment",
                          RecipientAccountNumber="1234 7654 9876 9874",
                          SenderAccountNumber="1000 1789 7890 6633",
                           RecordedOn=DateTime.Now,
                }
                };

            await _databaseAdapter.InsertAll(transactions);

            var metaData = new List<TransactionMetaData>()
            {
               new TransactionMetaData()
               {
                   ID = Guid.NewGuid().ToString(),
                    ReferenceID=id1,
                    AccountNumber ="1000 1789 7890 6633",
                    ClosingBalance=12000m
               },
                new TransactionMetaData()
               {
                   ID = Guid.NewGuid().ToString(),
                    ReferenceID=id2,
                    AccountNumber ="1000 1789 7890 6633",
                    ClosingBalance=14000m
               },
                 new TransactionMetaData()
               {
                   ID = Guid.NewGuid().ToString(),
                    ReferenceID=id3,
                    AccountNumber ="1000 1789 7890 6633",
                    ClosingBalance=9000m
               },
                    new TransactionMetaData()
               {
                   ID = Guid.NewGuid().ToString(),
                    ReferenceID=id3,
                    AccountNumber ="1666 5788 4567 5676",
                    ClosingBalance=100m
               },

            };

            await _databaseAdapter.InsertAll(metaData);

            List<ExternalAccount> externalAccounts = new List<ExternalAccount>()
            {
                new ExternalAccount()
                {
                    ExternalAccountNumber="1234 7654 9876 9874",
                    ExternalIFSCCode="HDFC1001",
                    ExternalName="Preethi",
                },
                 new ExternalAccount()
                {
                    ExternalAccountNumber="1235 4567 5678 6787",
                    ExternalIFSCCode="HDFC1002",
                    ExternalName="Govind",
                },
                  new ExternalAccount()
                {
                    ExternalAccountNumber="2002 7899 7890 9008",
                    ExternalIFSCCode="HDFC1004",
                    ExternalName="Yash Singh",
                },
                   new ExternalAccount()
                {
                    ExternalAccountNumber="6788 6788 7898 5890",
                    ExternalIFSCCode="HDFC1001",
                    ExternalName="Menon",
                }

            };

            await _databaseAdapter.InsertAll(externalAccounts);

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
                      CustomerID="80600504",
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
                      CustomerID="80600504",
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
                        ID="80600504",
                         LastLoggedOn=DateTime.Now,
                         Name="Kavya",
                          Phone="1234567890"
                },
                new Customer()
                {

                    ID="82248458",
                    Name="John",
                     Age=32,
                      LastLoggedOn=DateTime.Now,
                       Phone="1234567890",
                       Email="john@gmail.com",
                       CreatedOn=DateTime.Now,
                },
                new Customer()
                {
                     Age=23,
                      CreatedOn=DateTime.Now,
                       Email="kav@gmail.com",
                        ID="73506694",
                         LastLoggedOn=DateTime.Now,
                         Name="Henry",
                          Phone="1234567890"
                },
                new Customer()
                {

                    ID="37369343",
                    Name="Rose Mary",
                     Age=32,
                      LastLoggedOn=DateTime.Now,
                       Phone="1234567890",
                       Email="john@gmail.com",
                       CreatedOn=DateTime.Now,
                }
            };

            await _databaseAdapter.InsertAll(customers);

            List<CustomerCredentials> customerCredentials = new List<CustomerCredentials>()
            {
                new CustomerCredentials()
                {
                    ID="80600504",
                    Password="KXNtoOlsyUhNdfxB4B9UeSOFM+yr7UB5GOJL+0Ed2wkrVAS/voIxHlP22rYGVPu5FqRxXrx8C6fG/XhsrizRuWXzkW5KCg==",
                    Salt="OJ8r4j1wWa+bcFBJMM5B3Ik0Z3VuLfKyO6TCezKekwzutnnB7zWFxiyuFMbXF0HzrMDix8rfxzs4owbqfkiwm+Mpn3YktgdAMS0FRohhWMZDqfPAFD/tl3DV4OemeK7CkWgbgw=="
                },
                new CustomerCredentials()
                {
                    ID="82248458",
                    Password="O9zoKu4z/b+/nKO5Sitcjvccc7Gg/j64kKUSmnj4w5UVj9Xv3qvMwe9/TQ1hpCeMgR4uJoODb5kzYywepPq2P96jcsaHKw==",
                    Salt="07ESMtqq/jMGVGd8tsuRowELpvur+IPqEKBcuS4jXBCXiMw4C7E9HgZ+RjayBohsGUgHbqB14i5vBoK64bqYeGNAGYGAuBRvUclKIT9kEq+NOf159jf47lLvBc0rTcEzXAx2vw=="
                },
                new CustomerCredentials()
                {
                    ID="73506694",
                    Password="aM04c25M/TNFslg7jsLHypfH8QU1uxP7cS/fmAtcJ1ScK8aJpHBerAXbzOkwmj3NYcTpJAIe6t0G1c5rKvq1KomHMXbQwQ==",
                Salt="EjqHVbYKBlUXdjcDzKKBkpdWDmvk3x1j1adkG/oPE2AY1drXaxBLdmbwgYHRT9LPFsyprX1MZ6uy1KYCUyxz6UimE9pb0xBxCM/6PyXTpUPpd81LgwVZdawe9UVjaaB+dyqt8A=="
                },
                new CustomerCredentials()
                {
                    ID="37369343",
                    Password="w1F6KANAsfDKT28Qkq3f3BtSEL0/ksiCfNfHvE+gUbAsRC77fFOvFKoNd88jCfneP46BCFNbJKQyACagX3Vpyw9fzOzO7w==",
                    Salt="Z5eizKS4/7q+fTZHHwgax4oHiV/OxFR+M4qynARMl0f+fHzTmMXiLqUkYW65eX4+PBeISKpKc//bFCShbUuQhq20XlntrG/KuyZAqytSfWziO2wdkSkya6QsQOzGSwF43+YH1Q=="
                }
            };

            await _databaseAdapter.InsertAll(customerCredentials);

        }

        public async Task PayCreditCardBill(Transaction transaction, Account ownerAccount, TransactionMetaData metaData, CreditCard creditCard)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Execute("Update Account Set Balance = ? where AccountNumber = ?", ownerAccount.Balance, ownerAccount.AccountNumber);
                await _databaseAdapter.Execute("Update CreditCard Set TotalOutstanding = ? where CardNumber = ?", creditCard.TotalOutstanding, creditCard.CardNumber);
                await _databaseAdapter.Insert(transaction);
                await _databaseAdapter.Insert(metaData);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }


        public async Task<int> DeleteBeneficiary(Beneficiary beneficiary)
        {
            return await _databaseAdapter.Delete(beneficiary);
        }


        public Task<List<Branch>> GetBranchDetails()
        {
            return _databaseAdapter.GetAll<Branch>().ToListAsync();
        }

        public async Task InitiateTransactionExternal(Transaction transaction, Account ownerAccount, TransactionMetaData metaData)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Execute("Update Account Set Balance = ? where AccountNumber = ?", ownerAccount.Balance, ownerAccount.AccountNumber);
                await _databaseAdapter.Insert(transaction);
                await _databaseAdapter.Insert(metaData);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

        public async Task CloseDeposit(TermDepositAccount depositAccount ,Account repaymentAccount, Transaction transaction)
        {
            TermDepositAccountDTO depositAccountDTO = await _databaseAdapter.GetAll<TermDepositAccountDTO>().Where(acc => acc.AccountNumber == depositAccount.AccountNumber).FirstOrDefaultAsync();
            depositAccount.MaturityAmount = depositAccountDTO.MaturityAmount;   
            depositAccount.MaturityDate = depositAccountDTO.MaturityDate;

            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(transaction);
                await _databaseAdapter.Update(depositAccount, typeof(Account));
                await _databaseAdapter.Update(depositAccountDTO);
                await _databaseAdapter.Update(repaymentAccount, typeof(Account));
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

        public async Task<Account> GetAccountByAccountNumber(string customerID, string accountNumber)
        {
            return await _databaseAdapter.GetAll<Account>().Where(acc => acc.AccountNumber == accountNumber).FirstOrDefaultAsync();
        }

        public async Task<int> ResetPin(string cardNumber, string pin)
        {
            return await _databaseAdapter.Execute("Update Card Set Pin = ? where CardNumber = ?", pin, cardNumber);
        }


        public async Task InitiateTransactionInternal(Account ownerAccount, Account beneficiaryAccount, Transaction transaction, TransactionMetaData transactionMetaData, TransactionMetaData otherTransactionMetaData)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(transaction);
                await _databaseAdapter.Insert(transactionMetaData);
                await _databaseAdapter.Insert(otherTransactionMetaData);
                await _databaseAdapter.Execute("Update Account Set Balance = ? where AccountNumber = ?", ownerAccount.Balance, ownerAccount.AccountNumber);
                await _databaseAdapter.Execute("Update Account Set Balance = ? where AccountNumber = ?", beneficiaryAccount.Balance, beneficiaryAccount.AccountNumber);
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
                $"where UserID = ?", customerID);
            var savingsAccount = await _databaseAdapter.Query<SavingsAccount>($"Select * from Account " +
                $"Inner Join SavingsAccount on SavingsAccount.AccountNumber = Account.AccountNumber " +
                $"Inner Join Branch on Branch.IfscCode = Account.IFSCCode " +
                $"Left Join DebitCard on DebitCard.AccountNumber = Account.AccountNumber " +
                $"where UserID = ?", customerID);
            var termDepositAccounts = await _databaseAdapter.Query<TermDepositAccount>($"Select * from Account " +
                $"Inner Join TermDepositAccount on TermDepositAccount.AccountNumber = Account.AccountNumber " +
                $"Inner Join Branch on Branch.IfscCode = Account.IFSCCode " +
                $"where UserID = ?", customerID);

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
            await _databaseAdapter.CreateTable<TransactionMetaData>();
            await _databaseAdapter.CreateTable<ExternalAccount>();
            await _databaseAdapter.CreateTable<CreditCardDTO>();
            await _databaseAdapter.CreateTable<DebitCardDTO>();
            await _databaseAdapter.CreateTable<Bank>();
            await _databaseAdapter.CreateTable<Branch>();
            await _databaseAdapter.CreateTable<KYCDocuments>();
        }

        // Beneficiaries

        public async Task<IEnumerable<BeneficiaryBObj>> GetBeneficiaries(string customerID)
        {
            return await _databaseAdapter.Query<BeneficiaryBObj>($"Select Beneficiary.*, ExternalAccounts.ExternalIFSCCode, Account.IFSCCode from Beneficiary " +
                $"Left JOIN ExternalAccounts on ExternalAccounts.ExternalAccountNumber = Beneficiary.AccountNumber " +
                $"Left JOIN Account on Account.AccountNumber = Beneficiary.AccountNumber " +
                $"where Beneficiary.UserID = ?", customerID);
        }

        public Task<int> AddBeneficiary(Beneficiary beneficiary) => _databaseAdapter.Insert(beneficiary);

        public Task<int> UpdateBeneficiary(Beneficiary beneficiaryToUpdate) => _databaseAdapter.Update(beneficiaryToUpdate, typeof(Beneficiary));

        // Reset password

        public Task<int> ResetPassword(CustomerCredentials credentials) => _databaseAdapter.Update(credentials);


        // =========================================== \\ // ========================================== \\
        // Customer

        public async Task InsertCustomer(Customer customer, CustomerCredentials credentials)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(customer);
                await _databaseAdapter.Insert(credentials);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

        public Task<int> UpdateCustomer(Customer customer) => _databaseAdapter.Update(customer);

        public Task<List<Customer>> GetCustomer(string customerId) => _databaseAdapter.GetAll<Customer>().Where(customer => customerId.Equals(customer.ID)).ToListAsync();


        // Customer Credentials

        public async Task<CustomerCredentials> GetCredentials(string customerID)
        {
            return await _databaseAdapter.GetAll<CustomerCredentials>().Where(cred => cred.ID == customerID).FirstOrDefaultAsync();
        }

        public Task<int> InsertCredentials(CustomerCredentials customerCredentials) => _databaseAdapter.Insert(customerCredentials);

        public Task<int> UpdateCredentials(CustomerCredentials customerCredentials) => _databaseAdapter.Update(customerCredentials);



        public async Task UpdateAccount(TermDepositAccount account)
        {
            await _databaseAdapter.Execute("Update TermDepositAccount Set RepaymentAccountNumber = ? where AccountNumber = ?", account.RepaymentAccountNumber, account.AccountNumber);
        }

        //Card

        public Task<int> InsertCard(Card card) => _databaseAdapter.Insert(card);

        public async Task<int> UpdateCard(string cardNumber, decimal limit, string customerID) => await _databaseAdapter.Execute("Update Card Set SpendingLimit = ? Where CardNumber = ? And CustomerID = ? ", limit, cardNumber, customerID);


        public Task<int> InsertTransaction(Transaction transaction) => _databaseAdapter.Insert(transaction);

        public async Task<IEnumerable<TransactionBObj>> GetLatestMonthTransactionByAccountNumber(string accountNumber, string customerID)
        {
            var allTransactions = new List<TransactionBObj>();
            var incomeTransactions = await _databaseAdapter.Query<TransactionBObj>(
                incomingTransactionQuery + "AND RecordedOn < date('now','-30 days')", accountNumber, accountNumber);

            var expenseTransactions = await _databaseAdapter.Query<TransactionBObj>(
               expenseTransactionsQuery + " AND RecordedOn < date('now','-30 days')", accountNumber, accountNumber);

            allTransactions.AddRange(incomeTransactions);
            allTransactions.AddRange(expenseTransactions);
            return allTransactions.OrderByDescending(tran => tran.RecordedOn);
        }

        public async Task<IEnumerable<TransactionBObj>> FetchAllTodayTransactions(string accountNumber, string customerID)
        {
                var expenseTransactions = await _databaseAdapter.Query<TransactionBObj>(
                   expenseTransactionsQuery + " AND RecordedOn >= date('now', 'start of day')", accountNumber, accountNumber);

              return expenseTransactions.OrderByDescending(tran => tran.RecordedOn);
        }

        private string incomingTransactionQuery = "SELECT Transactions.*, TransactionMetaData.ClosingBalance, " +
                " ExternalAccounts.ExternalName, Beneficiary.BeneficiaryName, Account.AccountName FROM Transactions " +
                 "Left Join Beneficiary on Transactions.SenderAccountNumber = Beneficiary.AccountNumber " +
                 "Left Join Account on Transactions.SenderAccountNumber = Account.AccountNumber " +
                 "Left Join ExternalAccounts on Transactions.SenderAccountNumber = ExternalAccounts.ExternalAccountNumber " +
                "Left Join TransactionMetaData on Transactions.ReferenceID = TransactionMetaData.ReferenceID and TransactionMetaData.AccountNumber = ?" +
                "WHERE Transactions.RecipientAccountNumber == ? ";
        private string expenseTransactionsQuery = "SELECT Transactions.*, TransactionMetaData.ClosingBalance, ExternalAccounts.ExternalName, Account.AccountName, Beneficiary.BeneficiaryName FROM Transactions " +
                "Left Join Beneficiary on Transactions.RecipientAccountNumber = Beneficiary.AccountNumber " +
                 "Left Join Account on Transactions.SenderAccountNumber = Account.AccountNumber " +
                "Left Join ExternalAccounts on Transactions.RecipientAccountNumber = ExternalAccounts.ExternalAccountNumber " +
               "Left Join TransactionMetaData on Transactions.ReferenceID = TransactionMetaData.ReferenceID and TransactionMetaData.AccountNumber = ?" +
               "WHERE  Transactions.SenderAccountNumber == ? ";

        
        public async Task<IEnumerable<TransactionBObj>> GetAllTransactionByAccountNumber(string accountNumber, string customerID)
        {
            var allTransactions = new List<TransactionBObj>();

            var incomeTransactions = await _databaseAdapter.Query<TransactionBObj>(
                incomingTransactionQuery, accountNumber, accountNumber);

            var expenseTransactions = await _databaseAdapter.Query<TransactionBObj>(
            expenseTransactionsQuery, accountNumber, accountNumber);

            allTransactions.AddRange(incomeTransactions);
            allTransactions.AddRange(expenseTransactions);
            return allTransactions.OrderByDescending(tran => tran.RecordedOn);
        }

        public async Task InsertCreditCard(Card card, CreditCardDTO creditCardDTO)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(card);
                await _databaseAdapter.Insert(creditCardDTO);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

        public async Task InsertDebitCard(Card card, DebitCardDTO debitCardDTO)
        {
            Func<Task> action = async () =>
            {
                await _databaseAdapter.Insert(card);
                await _databaseAdapter.Insert(debitCardDTO);
            };
            await _databaseAdapter.RunInTransactionAsync(action);
        }

    }
}
