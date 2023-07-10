using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Data.DataManager.Contracts;
using ZBankManagement.Domain.UseCase;
using static ZBankManagement.Domain.UseCase.InitializeApp;

namespace ZBankManagement.Data.DataManager
{
    class InitializeAppDataManager : IInitializeAppDataManager
    {
        public InitializeAppDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task CreateTables(InitializeAppRequest request, IUseCaseCallback<InitializeAppResponse> callback)
        {
            try
            {
                await DBHandler.CreateTables().ConfigureAwait(false);
                callback.OnSuccess(new InitializeAppResponse());
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

        public async Task PopulateData(InitializeAppRequest request, IUseCaseCallback<InitializeAppResponse> callback)
        {
            try
            {
                await DBHandler.PopulateData().ConfigureAwait(false);
                callback.OnSuccess(new InitializeAppResponse());
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
