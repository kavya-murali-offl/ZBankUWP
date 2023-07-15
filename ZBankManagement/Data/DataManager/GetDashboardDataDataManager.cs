using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.BusinessObjects;
using ZBankManagement.Helpers;

namespace ZBank.ZBankManagement.DataLayer.DataManager
{
    class GetDashboardDataDataManager : IGetDashboardDataDataManager
    {
        private readonly IDBHandler _handler; 
        
        public GetDashboardDataDataManager(IDBHandler dbHandler) {
            _handler = dbHandler;   
        }

        public async Task GetDashboardData(GetDashboardDataRequest request, IUseCaseCallback<GetDashboardDataResponse> callback)
        {
            try
            {
                IEnumerable<AccountBObj> accountsList = await _handler.GetAllAccounts(request.UserID).ConfigureAwait(false);
                var BalanceCard = new DashboardCardModel
                {
                    PrimaryKey = "TotalBalance".GetLocalized(),
                    PrimaryValue = accountsList.Sum(acc => acc.Balance),
                    SecondaryKey1 = "TotalSavings".GetLocalized(),
                    SecondaryValue1 = accountsList.Where(acc => acc.AccountType != AccountType.TERM_DEPOSIT).Sum(acc => acc.Balance),
                    SecondaryKey2 = "TotalDeposits".GetLocalized(),
                    SecondaryValue2 = accountsList.Where(acc => acc.AccountType == AccountType.TERM_DEPOSIT).Sum(acc => acc.Balance)
                };

                IEnumerable<BeneficiaryBObj> beneficiaries = await _handler.GetBeneficiaries(request.UserID).ConfigureAwait(false);
                List<Branch> branches = await _handler.GetBranchDetails().ConfigureAwait(false);
                IEnumerable<string> ifscCodes = branches.Where(brn => brn.BankID == "1").Select(brn => brn.IfscCode);
                
                var BeneficiariesCard = new DashboardCardModel
                {
                    PrimaryKey = "TotalBeneficiaries".GetLocalized(),
                    PrimaryValue = beneficiaries.Count(),
                    SecondaryKey1 = "WithinBank".GetLocalized(),
                    SecondaryValue1 = beneficiaries.Where(ben => ifscCodes.Contains(ben.IFSCCode)).Count(),
                    SecondaryKey2 = "OtherBanks".GetLocalized(),
                    SecondaryValue2 = beneficiaries.Where(ben => !ifscCodes.Contains(ben.IFSCCode)).Count()
                };

                IEnumerable<TransactionBObj> transactions = new List<TransactionBObj>();
                decimal income = 0;
                decimal expense = 0;
                foreach(var account in accountsList)
                {
                    var list = await _handler.GetLatestMonthTransactionByAccountNumber(account.AccountNumber, request.UserID).ConfigureAwait(false);
                    foreach(var transaction in list)
                    {
                        if(transaction.RecipientAccountNumber == account.AccountNumber)
                        {
                            transaction.IsRecipient = true;
                        }
                        transactions = transactions.Append(transaction);
                    }
                    income += transactions.Where(tran => tran.IsRecipient).Sum(tran => tran.Amount);
                    expense += transactions.Where(tran => !tran.IsRecipient).Sum(tran => tran.Amount);
                }

                var IncomeExpenseCard = new DashboardCardModel
                {
                    PrimaryKey = "NetBalancePerMonth".GetLocalized(),
                    PrimaryValue = (income - expense),
                    SecondaryKey1 = "IncomePerMonth".GetLocalized(),
                    SecondaryValue1 = income,
                    SecondaryKey2 = "ExpensePerMonth".GetLocalized(),
                    SecondaryValue2 = expense
                };

                var deposits = accountsList.Where(acc => acc.AccountType == AccountType.TERM_DEPOSIT);
                var DepositCard = new DashboardCardModel
                {
                    PrimaryKey = "TotalDeposits".GetLocalized(),
                    PrimaryValue = deposits.Count(),
                    SecondaryKey1 = "ActiveDeposits".GetLocalized(),
                    SecondaryValue1 = deposits.Where(dep => dep.AccountStatus == AccountStatus.ACTIVE).Count(),
                    SecondaryKey2 = "ClosedDeposits".GetLocalized(),
                    SecondaryValue2 = deposits.Where(dep => dep.AccountStatus == AccountStatus.CLOSED).Count()
                };

                IEnumerable<CardBObj> AllCards = await _handler.GetAllCards(request.UserID).ConfigureAwait(false);
                transactions = transactions.Count() > 10 ? transactions.Take(10) : transactions;
                transactions = transactions.OrderByDescending(tran => tran.RecordedOn);
                GetDashboardDataResponse response = new GetDashboardDataResponse
                {
                    AllCards = AllCards,
                    DepositCard = DepositCard,
                    Beneficiaries = beneficiaries,
                    BeneficiariesCard = BeneficiariesCard,
                    Accounts = accountsList,
                    BalanceCard  = BalanceCard,
                    IncomeExpenseCard = IncomeExpenseCard,
                    LatestTransactions = transactions
                };

                callback.OnSuccess(response);   
            }
            catch (Exception ex)
            {
                ZBankException error = new ZBankException()
                {
                    Type = ErrorType.UNKNOWN,
                    Message = ex.Message,
                };
                callback.OnFailure(error);
            }
        }
    }
}
