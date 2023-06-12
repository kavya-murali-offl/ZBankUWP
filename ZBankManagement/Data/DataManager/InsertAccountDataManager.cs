using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Security.Principal;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    public class InsertAccountDataManager : IInsertAccountDataManager
    {

        public InsertAccountDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback)
        {
            try
            {
                bool result = await DBHandler.InsertAccount(request.AccountToInsert); ;
                if (result)
                {
                    InsertAccountResponse response = new InsertAccountResponse
                    {
                        IsSuccess = true,
                        InsertedAccount = request.AccountToInsert
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Error in inserting accounts";
                    error.Type = ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }
            }
            catch(Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }

    }
}



