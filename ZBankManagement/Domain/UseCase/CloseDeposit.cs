using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entity.BusinessObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Interface;
using ZBank.Entities;
using Microsoft.Extensions.DependencyInjection;
using ZBankManagement.Data.DataManager.Contracts;

namespace ZBankManagement.Domain.UseCase
{
    public class CloseDepositUseCase : UseCaseBase<CloseDepositResponse>
    {
        private readonly ICloseDepositDataManager closeDepositDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ICloseDepositDataManager>();
        private readonly CloseDepositRequest _request;

        public CloseDepositUseCase(CloseDepositRequest request, IPresenterCallback<CloseDepositResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }


        protected override void Action()
        {
            closeDepositDataManager.CloseDeposit(_request, new CloseDepositCallback(this));
        }

        private class CloseDepositCallback : IUseCaseCallback<CloseDepositResponse>
        {
            private readonly CloseDepositUseCase _useCase;

            public CloseDepositCallback(CloseDepositUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(CloseDepositResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class CloseDepositRequest : RequestObjectBase
    {
        public TermDepositAccount DepositAccount { get; set; }

        public string CardNumber { get; set; }
    }


    public class CloseDepositResponse
    {
        public TermDepositAccount DepositAccount { get; set; }
    }

}
