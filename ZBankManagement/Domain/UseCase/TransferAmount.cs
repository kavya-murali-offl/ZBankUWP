using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.DataManager;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class TransferAmountUseCase : UseCaseBase<TransferAmountResponse>
    {
        private readonly ITransferAmountDataManager _transferAmountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ITransferAmountDataManager>();
        private readonly TransferAmountRequest _request;

        public TransferAmountUseCase(TransferAmountRequest request, IPresenterCallback<TransferAmountResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }

        protected override void Action()
        {
            //_transferAmountDataManager.UpdateBalance(_request, new TransferAmountCallback(this));
        }

        private class TransferAmountCallback : IUseCaseCallback<TransferAmountResponse>
        {
            private readonly TransferAmountUseCase UseCase;

            public TransferAmountCallback(TransferAmountUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(TransferAmountResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                UseCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class TransferAmountRequest : RequestObjectBase
    {
        public Transaction Transaction { get; set; }
    }

    public class TransferAmountResponse
    {
        public bool IsSuccess { get; set; }

        public Card InsertedCard { get; set; }
    }

    public class TransferAmountPresenterCallback : IPresenterCallback<TransferAmountResponse>
    {

        public async Task OnSuccess(TransferAmountResponse response)
        {
        }

        public async Task OnFailure(ZBankException response)
        {

        }
    }
}
