using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Data;
using ZBankManagement.Data.DataManager.Contracts;

namespace ZBankManagement.Domain.UseCase
{
    public class InitializeApp
    {
        public class InitializeAppUseCase : UseCaseBase<InitializeAppResponse>
        {

            private readonly IInitializeAppDataManager _initializeAppDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInitializeAppDataManager>();
            private readonly InitializeAppRequest _request;

            public InitializeAppUseCase(InitializeAppRequest request, IPresenterCallback<InitializeAppResponse> presenterCallback)
                : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                IUseCaseCallback<InitializeAppResponse> callback;
                if(!File.Exists(Config.DatabasePath))
                {
                    callback = new CreateTableCallback(this);
                }
                else
                {
                    callback = new InitializeAppCallback(this);
                }
                _initializeAppDataManager.CreateTables(_request, callback);
            }

            private class InitializeAppCallback : IUseCaseCallback<InitializeAppResponse>
            {
                private readonly InitializeAppUseCase _useCase;

                public InitializeAppCallback(InitializeAppUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InitializeAppResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }

            private class CreateTableCallback : IUseCaseCallback<InitializeAppResponse>
            {
                private readonly InitializeAppUseCase _useCase;

                public CreateTableCallback(InitializeAppUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InitializeAppResponse response)
                {
                    _useCase._initializeAppDataManager.PopulateData(_useCase._request, new InitializeAppCallback(_useCase));
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InitializeAppRequest : RequestObjectBase
        {
        }


        public class InitializeAppResponse
        {
            public IEnumerable<Branch> BranchList { get; set; }
        }
    }
}
