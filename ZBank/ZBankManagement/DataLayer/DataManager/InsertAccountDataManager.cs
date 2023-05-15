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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                await DBHandler.InsertAccount(request.AccountToInsert);
                InsertAccountResponse response = new InsertAccountResponse
                {
                    IsSuccess = true,
                    InsertedAccount = request.AccountToInsert
                };
                callback.OnSuccess(response);
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



