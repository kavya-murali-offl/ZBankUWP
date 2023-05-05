using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using BankManagementDB.Interface;

namespace BankManagementDB.Controller
{
    public class AccountFactory : IAccountFactory
    {
        public Account GetAccountByType(AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.CURRENT:
                    {
                        CurrentAccount currentAccount = new CurrentAccount
                        {
                            InterestRate = 0
                        };

                        return currentAccount;
                    }
                case AccountType.SAVINGS:
                    {
                        SavingsAccount savingsAccount = new SavingsAccount
                        {
                            InterestRate = 3.1m
                        };

                        return savingsAccount;
                    }
                default:
                    return null;
            }
        }

     
    }
}
