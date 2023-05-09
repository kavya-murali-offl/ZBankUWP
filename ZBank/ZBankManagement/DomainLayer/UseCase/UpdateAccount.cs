using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class UpdateAccount
    {
        public class UpdateAccountUseCase<T> : UseCaseBase<UpdateAccountRequest<T>, UpdateAccountResponse<T>>
        {
            private readonly IUpdateAccountDataManager UpdateAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateAccountDataManager>();

            private IPresenterCallback<UpdateAccountResponse<T>> PresenterCallback;

            protected override void Action(UpdateAccountRequest<T> request, IPresenterCallback<UpdateAccountResponse<T>> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                UpdateAccountDataManager.UpdateAccount<T>(request, new UpdateAccountCallback(this));
            }

            private class UpdateAccountCallback : IUseCaseCallback<UpdateAccountResponse<T>>
            {

                UpdateAccountUseCase<T> UseCase;

                public UpdateAccountCallback(UpdateAccountUseCase<T> useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(UpdateAccountResponse<T> response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateAccountRequest<T>
        {
            public T UpdatedAccount { get; set; }
        }

        public class UpdateAccountResponse<T>
        {
            public bool IsSuccess { get; set; }

            public T UpdatedAccount { get; set; }
        }

        public class UpdateAccountPresenterCallback<T> : IPresenterCallback<UpdateAccountResponse<T>>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public UpdateAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(UpdateAccountResponse<T> response)
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
