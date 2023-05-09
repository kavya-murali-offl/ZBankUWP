using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
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
    public class GetAllCardsUseCase : UseCaseBase<GetAllCardsRequest, GetAllCardsResponse>
    {
        private readonly IGetCardDataManager GetCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetCardDataManager>();

        private IPresenterCallback<GetAllCardsResponse> PresenterCallback;

        protected override void Action(GetAllCardsRequest request, IPresenterCallback<GetAllCardsResponse> presenterCallback)
        {
            PresenterCallback = presenterCallback;
            GetCardDataManager.GetAllCards(request, new GetAllCardsCallback(this));
        }

        private class GetAllCardsCallback : IUseCaseCallback<GetAllCardsResponse>
        {

            GetAllCardsUseCase UseCase;

            public GetAllCardsCallback(GetAllCardsUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(GetAllCardsResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                UseCase.PresenterCallback.OnFailure(error);
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
        private AccountPageViewModel AccountPageViewModel { get; set; }

        public GetAllCardsPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
            AccountPageViewModel = accountPageViewModel;
        }

        public async void OnSuccess(GetAllCardsResponse response)
        {
            await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
            });
        }

        public void OnFailure(ZBankError response)
        {

        }
    }
}
