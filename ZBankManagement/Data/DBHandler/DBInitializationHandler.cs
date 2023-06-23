using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using ZBank.DatabaseAdapter;
using ZBank.Entity;
using ZBank.Entities.EnumerationType;
using ZBank.Entity.EnumerationTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZBankManagement.Entity.DTOs;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.DataLayer.DBHandler
{
    class DBInitializationHandler : IDBInitializationHandler
    {
        private IDatabaseAdapter _databaseAdapter { get; set; }

        public DBInitializationHandler(IDatabaseAdapter _dbAdapter)
        {
            _databaseAdapter = _dbAdapter;
        }

        public void CreateTables()
        {
            _databaseAdapter.CreateTable<Customer>();
            _databaseAdapter.CreateTable<CustomerCredentials>();
            _databaseAdapter.CreateTable<Card>();
            _databaseAdapter.CreateTable<Account>();
            _databaseAdapter.CreateTable<Beneficiary>();
            _databaseAdapter.CreateTable<CurrentAccountDTO>();
            _databaseAdapter.CreateTable<SavingsAccountDTO>();
            _databaseAdapter.CreateTable<TermDepositAccountDTO>();
            _databaseAdapter.CreateTable<Transaction>();
            _databaseAdapter.CreateTable<CreditCardDTO>();
            _databaseAdapter.CreateTable<DebitCardDTO>();
            _databaseAdapter.CreateTable<Bank>();
            _databaseAdapter.CreateTable<Branch>();
            _databaseAdapter.CreateTable<KYCDocuments>();
        }

        public  void PopulateData()
        {
            _databaseAdapter.Insert(new Bank()
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

            var insert1 = _databaseAdapter.InsertAll(currentAccountDTOs);

            List<SavingsAccountDTO> savingsAccountDTOs = new List<SavingsAccountDTO>()
            {
                new SavingsAccountDTO()
                {
                    AccountNumber="1001 1000 1789 7890",
                    InterestRate=5.4m,
                }
            };

            var insert2 = _databaseAdapter.InsertAll(savingsAccountDTOs);

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

            var insert3 = _databaseAdapter.InsertAll(termDepositAccountDTOs);

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

            _databaseAdapter.InsertAll(accounts);


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
            _databaseAdapter.InsertAll(beneficiaries);

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
            _databaseAdapter.InsertAll(branches);

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

            _databaseAdapter.InsertAll(cards);

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

            _databaseAdapter.InsertAll(creditCardDTOs);

            List<DebitCardDTO> debitCardDTOs = new List<DebitCardDTO>()
            {
                new DebitCardDTO()
                {
                    CardNumber="1245 6888 0087 56788",
                    AccountNumber="1000 1789 7890 6633",
                }
            };

            _databaseAdapter.InsertAll(debitCardDTOs);

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

            _databaseAdapter.InsertAll(customers);

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

            _databaseAdapter.InsertAll(transactions);

        }
    }
}
