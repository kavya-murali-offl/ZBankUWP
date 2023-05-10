using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using LiveCharts.Events;
using System;
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
            bool result = false;

            try
            {
                if (request.AccountToInsert is CurrentAccount)
                {
                    CurrentAccount account = request.AccountToInsert as CurrentAccount;

                    CurrentAccountDTO dtoAccount = new CurrentAccountDTO();
                    dtoAccount.AccountNumber = account.AccountNumber;
                    dtoAccount.Interest = account.InterestRate;

                    DBHandler.InsertAccount(dtoAccount, request.AccountToInsert);
                }
                //}
                //else if (request.AccountToInsert is SavingsAccount)
                //{
                //    SavingsAccount account = request.AccountToInsert as SavingsAccount;

                //    SavingsAccountDTO dtoAccount = new SavingsAccountDTO();
                //    dtoAccount.AccountNumber = account.AccountNumber;
                //    dtoAccount.Interest = account.InterestRate;

                //    DBHandler.InsertAccount(dtoAccount, request.AccountToInsert);
                //}
                //else if (request.AccountToInsert is TermDepositAccount)
                //{
                //    TermDepositAccountDTO dtoAccount = new TermDepositAccountDTO();
                //    dtoAccount.AccountNumber = request.AccountToInsert.AccountNumber;
                //    DBHandler.InsertAccount(dtoAccount, request.AccountToInsert);
                //}
                //result = true;
            }
            catch(Exception err)
            {
                result = false;
            }
            
            if (result)
            {
                InsertAccountResponse response = new InsertAccountResponse();
                response.IsSuccess = result;
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



