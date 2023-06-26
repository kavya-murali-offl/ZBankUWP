using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Data.DataManager.Contracts;

namespace ZBankManagement.Domain.UseCase
{
    public class ResetPin
    {
        public class ResetPinUseCase : UseCaseBase<ResetPinResponse>
        {
            private readonly IResetPinDataManager _resetPinDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IResetPinDataManager>();
            private readonly ResetPinRequest _request;

            public ResetPinUseCase(ResetPinRequest request, IPresenterCallback<ResetPinResponse> presenterCallback)
                : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _resetPinDataManager.ResetPin(_request, new ResetPinCallback(this));
            }

            private class ResetPinCallback : IUseCaseCallback<ResetPinResponse>
            {

                private readonly ResetPinUseCase _useCase;

                public ResetPinCallback(ResetPinUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(ResetPinResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class ResetPinRequest : RequestObjectBase
        {
            public string CardNumber { get; set; }
            public string NewPin { get; set; }
        }

        public class ResetPinResponse
        {
            public bool IsSuccess { get; set; }
        }
    }
}
