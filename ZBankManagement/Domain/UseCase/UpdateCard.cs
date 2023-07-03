using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class UpdateCard
    {
        public class UpdateCardUseCase : UseCaseBase<UpdateCardResponse>
        {
            private readonly IUpdateCardDataManager _updateCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCardDataManager>();
            private readonly UpdateCardRequest _request;

            public UpdateCardUseCase(UpdateCardRequest request, IPresenterCallback<UpdateCardResponse> presenterCallback)
                : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _updateCardDataManager.UpdateCard(_request, new UpdateCardCallback(this));
            }

            private class UpdateCardCallback : IUseCaseCallback<UpdateCardResponse>
            {

                private readonly UpdateCardUseCase _useCase;

                public UpdateCardCallback(UpdateCardUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(UpdateCardResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateCardRequest : RequestObjectBase
        {
            public Card CardToUpdate { get; set; }
        }

        public class UpdateCardResponse
        {
            public bool IsSuccess { get; set; }

            public Card UpdatedCard { get; set; }
        }

    }
}
