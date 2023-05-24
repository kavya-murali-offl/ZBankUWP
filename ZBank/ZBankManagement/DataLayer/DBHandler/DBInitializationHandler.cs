using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.DatabaseAdapter;
using ZBank.Entity;

namespace ZBank.ZBankManagement.DataLayer.DBHandler
{
    public class DBInitializationHandler : IDBInitializationHandler
    {
        private IDatabaseAdapter _databaseAdapter { get; set; }

        public DBInitializationHandler(IDatabaseAdapter _dbAdapter) {
            _databaseAdapter = _dbAdapter;
        }

        public void CreateTables()
        {
            _databaseAdapter.CreateTable<Customer>();
            _databaseAdapter.CreateTable<CustomerCredentials>();
            _databaseAdapter.CreateTable<Card>();
            _databaseAdapter.CreateTable<AccountDTO>();
            _databaseAdapter.CreateTable<Beneficiary>();
            _databaseAdapter.CreateTable<CurrentAccountDTO>();
            _databaseAdapter.CreateTable<SavingsAccountDTO>();
            _databaseAdapter.CreateTable<TermDepositAccountDTO>();
            _databaseAdapter.CreateTable<Transaction>();
            _databaseAdapter.CreateTable<CreditCardDTO>();
            _databaseAdapter.CreateTable<DebitCardDTO>();
            _databaseAdapter.CreateTable<Bank>();
            _databaseAdapter.CreateTable<Branch>();
        }

    }
}
