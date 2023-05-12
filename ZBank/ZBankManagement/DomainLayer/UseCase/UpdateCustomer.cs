using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class UpdateCustomerUseCase : UseCaseBase<UpdateCustomerResponse>
        {
            private readonly IUpdateCustomerDataManager _updateCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCustomerDataManager>();
            private readonly UpdateCustomerRequest _request;
            private readonly IPresenterCallback<UpdateCustomerResponse> _presenterCallback;

            public UpdateCustomerUseCase(UpdateCustomerRequest request, IPresenterCallback<UpdateCustomerResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
                _request = request;
            }

            protected override void Action()
            {
                _updateCustomerDataManager.UpdateCustomer(_request, new UpdateCustomerCallback(this));
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
                    UseCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    UseCase._presenterCallback.OnFailure(error);
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

            public void OnSuccess(UpdateCustomerResponse response)
            {
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
}
