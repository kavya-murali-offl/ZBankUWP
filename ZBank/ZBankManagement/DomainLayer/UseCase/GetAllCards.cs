using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
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

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllCardsUseCase : UseCaseBase<GetAllCardsResponse>
    {
        private readonly IGetCardDataManager _getCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetCardDataManager>();
        private readonly IPresenterCallback<GetAllCardsResponse> _presenterCallback;
        private readonly GetAllCardsRequest _request;

        public GetAllCardsUseCase(GetAllCardsRequest request, IPresenterCallback<GetAllCardsResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
            _request = request;
        }

        protected override void Action()
        {
            _getCardDataManager.GetAllCards(_request, new GetAllCardsCallback(this));
        }

        private class GetAllCardsCallback : IUseCaseCallback<GetAllCardsResponse>
        {
            private readonly GetAllCardsUseCase _useCase;

            public GetAllCardsCallback(GetAllCardsUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllCardsResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllCardsRequest
    {

        public string CustomerID { get; set; }
    }


    public class GetAllCardsResponse
    {
        public IEnumerable<Card> Cards { get; set; }
    }


    public class GetAllCardsPresenterCallback : IPresenterCallback<GetAllCardsResponse>
    {

        public void OnSuccess(GetAllCardsResponse response)
        {
        }

        public void OnFailure(ZBankError response)
        {

        }
    }
}
