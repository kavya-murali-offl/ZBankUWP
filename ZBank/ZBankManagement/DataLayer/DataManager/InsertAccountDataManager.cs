using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using LiveCharts.Events;
using System.Security.Principal;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using static SQLite.SQLite3;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertAccount;

namespace BankManagementDB.DataManager
{
    public class InsertAccountDataManager : IInsertAccountDataManager
    {
        public InsertAccountDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback)
        {
            bool result =  DBHandler.InsertAccount(request.AccountToInsert).Result;

            InsertAccountResponse response = new InsertAccountResponse();
            response.IsSuccess = result;

            if (result)
            {
                response.InsertedAccount = request.AccountToInsert;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankError error = new ZBankError();
                error.Message = "Account Not Inserted";
                error.Type = ErrorType.UNKNOWN;
                callback.OnFailure(error);
            }
        }
    }
}


//if (request.Account is CurrentAccount)
//{
//    CurrentAccount account = request.Account as CurrentAccount;

//    CurrentAccountDTO dtoAccount = new CurrentAccountDTO();
//    dtoAccount.AccountNumber = account.AccountNumber;
//    dtoAccount.Interest = account.InterestRate;

//    result = DBHandler.InsertAccount(dtoAccount, account).Result;
//}
//else if (request.Account is SavingsAccount)
//{
//    SavingsAccount account = request.Account as SavingsAccount;

//    SavingsAccountDTO dtoAccount = new SavingsAccountDTO();
//    dtoAccount.AccountNumber = account.AccountNumber;
//    dtoAccount.Interest = account.InterestRate;

//    result = DBHandler.InsertAccount(dtoAccount, request.Account).Result;
//}
//else if (request.Account is TermDepositAccount)
//{
//    TermDepositAccountDTO dtoAccount = new TermDepositAccountDTO();
//    dtoAccount.AccountNumber = request.Account.AccountNumber;
//    result = DBHandler.InsertAccount(dtoAccount, request.Account).Result;
//}
