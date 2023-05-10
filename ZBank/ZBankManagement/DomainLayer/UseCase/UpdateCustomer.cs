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
    public class UpdateCustomer
    {
        public class UpdateCustomerUseCase : UseCaseBase<UpdateCustomerRequest, UpdateCustomerResponse>
        {
            private readonly IUpdateCustomerDataManager UpdateCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCustomerDataManager>();

            private IPresenterCallback<UpdateCustomerResponse> PresenterCallback;

            protected override void Action(UpdateCustomerRequest request, IPresenterCallback<UpdateCustomerResponse> presenterCallback)
            {
                PresenterCallback = presenterCallback;
                UpdateCustomerDataManager.UpdateCustomer(request, new UpdateCustomerCallback(this));
            }

            private class UpdateCustomerCallback : IUseCaseCallback<UpdateCustomerResponse>
            {

                UpdateCustomerUseCase UseCase;

                public UpdateCustomerCallback(UpdateCustomerUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(UpdateCustomerResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateCustomerRequest
        {
            public Customer CustomerToUpdate { get; set; }
        }

        public class UpdateCustomerResponse
        {
            public bool IsSuccess { get; set; }

            public Account InsertedAccount { get; set; }
        }

        public class UpdateCustomerPresenterCallback : IPresenterCallback<UpdateCustomerResponse>
        {

            public async void OnSuccess(UpdateCustomerResponse response)
            {
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
