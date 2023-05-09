using BankManagementDB.DataManager;
using BankManagementDB.Domain.UseCase;
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
    public class InsertCard
    {
        public class InsertCardUseCase : UseCaseBase<InsertCardRequest, InsertCardResponse>
        {
            private readonly IInsertCardDataManager InsertCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertCardDataManager>();

            private IPresenterCallback<InsertCardResponse> PresenterCallback;

            protected override void Action(InsertCardRequest request, IPresenterCallback<InsertCardResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                InsertCardDataManager.InsertCard(request, new InsertCardCallback(this));
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

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertCardRequest
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
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertCardPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(InsertCardResponse response)
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
}
