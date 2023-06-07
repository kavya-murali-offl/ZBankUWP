using ZBank.Entities.EnumerationType;
using ZBank.Entities;
using ZBankManagement.Interface;
using System;
using System.Security.Principal;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;

namespace ZBankManagement.Controller
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

        public static object GetDTOObject(Account account)
        {
            switch (account)
            {
                case CurrentAccount _:
                    return Mapper.Map<CurrentAccount, CurrentAccountDTO>(account as CurrentAccount);

                case SavingsAccount _:
                    return Mapper.Map<SavingsAccount, SavingsAccountDTO>(account as SavingsAccount);

                case TermDepositAccount _:
                    return Mapper.Map<TermDepositAccount, TermDepositAccountDTO>(account as TermDepositAccount);

                default:
                    return null;
            }
        }
    }
}
