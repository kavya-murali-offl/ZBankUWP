using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using LiveCharts.Events;
using System;
using System.Security.Principal;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
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
            try
            {
                DBHandler.InsertAccount(request.AccountToInsert);
                InsertAccountResponse response = new InsertAccountResponse();
                response.IsSuccess = true;
                response.InsertedAccount = request.AccountToInsert;
                callback.OnSuccess(response);
            }
            catch(Exception err)
            {
                ZBankError error = new ZBankError();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }

        }
    }
}



