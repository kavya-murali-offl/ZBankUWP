using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class UpdateCard
    {
        public class UpdateCardUseCase : UseCaseBase<UpdateCardResponse>
        {
            private readonly IUpdateCardDataManager _updateCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCardDataManager>();
            private readonly UpdateCardRequest _request;
            private readonly IPresenterCallback<UpdateCardResponse> _presenterCallback;

            public UpdateCardUseCase(UpdateCardRequest request, IPresenterCallback<UpdateCardResponse> presenterCallback)
            {
                _request = request;
                _presenterCallback = presenterCallback;
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
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateCardRequest
        {
            public Card UpdatedCard { get; set; }
        }

        public class UpdateCardResponse
        {
            public bool IsSuccess { get; set; }

            public Card UpdatedCard { get; set; }
        }

        public class UpdateCardPresenterCallback : IPresenterCallback<UpdateCardResponse>
        {

            public void OnSuccess(UpdateCardResponse response)
            {
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
