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
using ZBank.ViewModel;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Utility;

namespace ZBank.ZBankManagement.DataLayer.DataManager
{
    public class GetDashboardDataDataManager : IGetDashboardDataDataManager
    {
        private readonly IDBHandler _handler; 
        
        public GetDashboardDataDataManager(IDBHandler dbHandler) {
            _handler = dbHandler;   
        }

        public void GetDashboardData(GetDashboardDataRequest request, IUseCaseCallback<GetDashboardDataResponse> callback)
        {
            try
            {
                IEnumerable<AccountBObj> accountsList = _handler.GetAllAccounts(request.UserID).Result;
                var BalanceCard = new DashboardCardModel
                {
                    PrimaryKey = "Total Balance",
                    PrimaryValue = accountsList.Sum(acc => acc.Amount),
                    SecondaryKey1 = "Total Savings",
                    SecondaryValue1 = accountsList.Where(acc => acc.AccountType != AccountType.TERM_DEPOSIT).Sum(acc => acc.Amount),
                    SecondaryKey2 = "Total Deposits",
                    SecondaryValue2 = accountsList.Where(acc => acc.AccountType == AccountType.TERM_DEPOSIT).Sum(acc => acc.Amount)
                };

                IEnumerable<Beneficiary> beneficiaries = _handler.GetBeneficiaries(request.UserID).Result;
                IEnumerable<string> ifscCodes = _handler.GetBranchDetails().Result.Where(brn => brn.BankID == "1").Select(brn => brn.IfscCode);
                var BeneficiariesCard = new DashboardCardModel
                {
                    PrimaryKey = "Total Beneficiaries",
                    PrimaryValue = beneficiaries.Count(),
                    SecondaryKey1 = "Within Bank",
                    SecondaryValue1 = beneficiaries.Where(ben => ifscCodes.Contains(ben.IFSCCode)).Count(),
                    SecondaryKey2 = "Other Banks",
                    SecondaryValue2 = beneficiaries.Where(ben => !ifscCodes.Contains(ben.IFSCCode)).Count()
                };


                List<TransactionBObj> transactions = new List<TransactionBObj>();
                foreach(var account in accountsList)
                {
                    var list = _handler.GetLatestMonthTransactionByAccountNumber(account.AccountNumber).Result;
                    transactions.AddRange(list);
                }

                var income = transactions.Where(tran => tran.TransactionType == Entities.TransactionType.INCOME).Sum(tran => tran.Amount);
                var expense = transactions.Where(tran => tran.TransactionType == Entities.TransactionType.EXPENSE).Sum(tran => tran.Amount);

                var IncomeExpenseCard = new DashboardCardModel
                {
                    PrimaryKey = "Net Profit / Month",
                    PrimaryValue = (income - expense),
                    SecondaryKey1 = "Income / Month",
                    SecondaryValue1 = income,
                    SecondaryKey2 = "Expense / Month",
                    SecondaryValue2 = expense
                };

                var deposits = accountsList.Where(acc => acc.AccountType == AccountType.TERM_DEPOSIT);
                var DepositCard = new DashboardCardModel
                {
                    PrimaryKey = "Total Deposits",
                    PrimaryValue = deposits.Count(),
                    SecondaryKey1 = "Active Deposits",
                    SecondaryValue1 = deposits.Where(dep => dep.AccountStatus == AccountStatus.ACTIVE).Count(),
                    SecondaryKey2 = "Closed Deposits",
                    SecondaryValue2 = deposits.Where(dep => dep.AccountStatus == AccountStatus.CLOSED).Count()
                };

                IEnumerable<CardBObj> AllCards = _handler.GetAllCards(request.UserID).Result;

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
