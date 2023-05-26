using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.Entities.BusinessObjects;
using System.Collections.ObjectModel;
using System;
using Windows.UI.Core;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using Windows.UI.Popups;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllCardsUseCase : UseCaseBase<GetAllCardsResponse>
    {
        private readonly IGetCardDataManager _getCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetCardDataManager>();
        private readonly GetAllCardsRequest _request;

        public GetAllCardsUseCase(GetAllCardsRequest request, IPresenterCallback<GetAllCardsResponse> presenterCallback) 
            : base(presenterCallback, request.Token)
        {
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
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllCardsRequest : RequestObjectBase
    {
        public string CustomerID { get; set; }
    }


    public class GetAllCardsResponse
    {
        public IEnumerable<Card> Cards { get; set; }
    }


    public class GetAllCardsPresenterCallback : IPresenterCallback<GetAllCardsResponse>
    {
        private readonly CardsViewModel ViewModel;

        public GetAllCardsPresenterCallback(CardsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllCardsResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CardPageDataUpdatedArgs args = new CardPageDataUpdatedArgs()
                {
                    CardsList = new ObservableCollection<Card>(response.Cards)
                };
                ViewNotifier.Instance.OnCardsPageDataUpdated(args);
            });
        }

        public async void OnFailure(ZBankException exception)
        {
            var dialog = new MessageDialog(exception.Message);
            await dialog.ShowAsync();

        }
    }
}
