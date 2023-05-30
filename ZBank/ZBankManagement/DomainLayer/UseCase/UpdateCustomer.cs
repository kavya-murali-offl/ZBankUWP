using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class UpdateCustomerUseCase : UseCaseBase<UpdateCustomerResponse>
        {
            private readonly IUpdateCustomerDataManager _updateCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCustomerDataManager>();
            private readonly UpdateCustomerRequest _request;

            public UpdateCustomerUseCase(UpdateCustomerRequest request, IPresenterCallback<UpdateCustomerResponse> callback) 
                : base(callback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _updateCustomerDataManager.UpdateCustomer(_request, new UpdateCustomerCallback(this));
            }

            private class UpdateCustomerCallback : IUseCaseCallback<UpdateCustomerResponse>
            {
                private readonly UpdateCustomerUseCase UseCase;

                public UpdateCustomerCallback(UpdateCustomerUseCase useCase)
                {
                    UseCase = useCase;
                }

                public void OnSuccess(UpdateCustomerResponse response)
                {
                    UseCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    UseCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateCustomerRequest : RequestObjectBase
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

            public void OnSuccess(UpdateCustomerResponse response)
            {
            }

            public void OnFailure(ZBankException response)
            {

            }
        }
}
