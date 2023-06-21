using ZBankManagement.Interface;
using ZBank.Entities;
using System.Collections.Generic;
using ZBank.DatabaseHandler;
using System.Linq;
using ZBank.Entities.EnumerationType;
using ZBankManagement.Domain.UseCase;
using System;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Diagnostics;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    class GetAccountDataManager : IGetAccountDataManager
    {
        public GetAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task GetAllAccounts(GetAllAccountsRequest request, IUseCaseCallback<GetAllAccountsResponse> callback)
        {
            try
            {
                IEnumerable<AccountBObj> accountsList = await DBHandler.GetAllAccounts(request.UserID);
                GetAllAccountsResponse response = new GetAllAccountsResponse()
                {
                    Accounts = accountsList
                };
                callback.OnSuccess(response);
            }
            catch(Exception ex)
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
