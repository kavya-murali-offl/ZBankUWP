using ZBankManagement.DataManager;
using ZBankManagement.Domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertCardUseCase : UseCaseBase<InsertCardResponse>
        {
            private readonly IInsertCardDataManager _insertCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertCardDataManager>();
            private readonly InsertCardRequest _request;

            public InsertCardUseCase(InsertCardRequest request, IPresenterCallback<InsertCardResponse> presenterCallback)
                : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _insertCardDataManager.InsertCard(_request, new InsertCardCallback(this));
            }

            private class InsertCardCallback : IUseCaseCallback<InsertCardResponse>
            {
                private readonly InsertCardUseCase UseCase;

                public InsertCardCallback(InsertCardUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(InsertCardResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertCardRequest : RequestObjectBase
        {
            public Card CardToInsert { get; set; }
        }

        public class InsertCardResponse
        {
            public bool IsSuccess { get; set; }

            public Card InsertedCard { get; set; }
        }

        public class InsertCardPresenterCallback : IPresenterCallback<InsertCardResponse>
        {

            public void OnSuccess(InsertCardResponse response)
            {
            }

            public void OnFailure(ZBankException response)
            {

            }
    }
}
