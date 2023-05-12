using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class UpdateAccountUseCase : UseCaseBase<UpdateAccountResponse>
        {
            private readonly IUpdateAccountDataManager _updateAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateAccountDataManager>();
            private readonly IPresenterCallback<UpdateAccountResponse> _presenterCallback;
            private readonly UpdateAccountRequest _request;

            public UpdateAccountUseCase(UpdateAccountRequest request, IPresenterCallback<UpdateAccountResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
                _request = request;

            }
            protected override void Action()
            {
                _updateAccountDataManager.UpdateAccount(_request, new UpdateAccountCallback(this));
            }

            private class UpdateAccountCallback : IUseCaseCallback<UpdateAccountResponse>
            {

                private readonly UpdateAccountUseCase _useCase;

                public UpdateAccountCallback(UpdateAccountUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(UpdateAccountResponse response)
                {
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateAccountRequest
        {
            public Account UpdatedAccount { get; set; }
        }

        public class UpdateAccountResponse
        {
            public bool IsSuccess { get; set; }
            public Account UpdatedAccount { get; set; }
        }

        public class UpdateAccountPresenterCallback : IPresenterCallback<UpdateAccountResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public UpdateAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(UpdateAccountResponse response)
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
