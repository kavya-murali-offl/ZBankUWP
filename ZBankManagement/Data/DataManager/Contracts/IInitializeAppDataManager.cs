using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ZBankManagement.Domain.UseCase.InitializeApp;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Data.DataManager.Contracts
{
    interface IInitializeAppDataManager
    {
        Task CreateTables(InitializeAppRequest request, IUseCaseCallback<InitializeAppResponse> callback);
        Task PopulateData(InitializeAppRequest request, IUseCaseCallback<InitializeAppResponse> callback);
    }
}
