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
    public class InsertCustomer
    {
        public class InsertCustomerUseCase : UseCaseBase<InsertCustomerRequest, InsertCustomerResponse>
        {
            private readonly IInsertCustomerDataManager InsertCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertCustomerDataManager>();

            private IPresenterCallback<InsertCustomerResponse> PresenterCallback;

            protected override void Action(InsertCustomerRequest request, IPresenterCallback<InsertCustomerResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                InsertCustomerDataManager.InsertCustomer(request, new InsertCustomerCallback(this));
            }

            private class InsertCustomerCallback : IUseCaseCallback<InsertCustomerResponse>
            {
                private readonly InsertCustomerUseCase UseCase;

                public InsertCustomerCallback(InsertCustomerUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(InsertCustomerResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertCustomerRequest
        {
            public Customer CustomerToInsert { get; set; }

            public string Password { get; set; }
        }

        public class InsertCustomerResponse
        {
            public bool IsSuccess { get; set; }

            public Customer InsertedCustomer { get; set; }
        }

        public class InsertCustomerPresenterCallback : IPresenterCallback<InsertCustomerResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertCustomerPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(InsertCustomerResponse response)
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
