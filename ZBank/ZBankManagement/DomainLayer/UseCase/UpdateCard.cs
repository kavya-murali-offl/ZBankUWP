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
    public class UpdateCard
    {
        public class UpdateCardUseCase : UseCaseBase<UpdateCardRequest, UpdateCardResponse>
        {
            private readonly IUpdateCardDataManager UpdateCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCardDataManager>();

            private IPresenterCallback<UpdateCardResponse> PresenterCallback;

            protected override void Action(UpdateCardRequest request, IPresenterCallback<UpdateCardResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                UpdateCardDataManager.UpdateCard(request, new UpdateCardCallback(this));
            }

            private class UpdateCardCallback : IUseCaseCallback<UpdateCardResponse>
            {

                UpdateCardUseCase UseCase;

                public UpdateCardCallback(UpdateCardUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(UpdateCardResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
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
            private AccountPageViewModel CardPageViewModel { get; set; }

            public UpdateCardPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
            }

            public async void OnSuccess(UpdateCardResponse response)
            {
                //await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //{
                //    var accounts = CardPageViewModel.Cards.Prepend(response.UpdatedCard);
                //    CardPageViewModel.Cards = new ObservableCollection<Card>(accounts);
                //});
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
