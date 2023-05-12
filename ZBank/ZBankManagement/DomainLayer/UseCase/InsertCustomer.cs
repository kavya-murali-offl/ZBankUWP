using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertCustomerUseCase : UseCaseBase<InsertCustomerResponse>
        {
            private readonly IInsertCustomerDataManager _insertCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertCustomerDataManager>();
            private readonly InsertCustomerRequest _request;
            private readonly IPresenterCallback<InsertCustomerResponse> _presenterCallback;

            public InsertCustomerUseCase(InsertCustomerRequest request, IPresenterCallback<InsertCustomerResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
                _request = request;
            }


            protected override void Action()
            {
                _insertCustomerDataManager.InsertCustomer(_request, new InsertCustomerCallback(this));
            }

            private class InsertCustomerCallback : IUseCaseCallback<InsertCustomerResponse>
            {
                private readonly InsertCustomerUseCase _useCase;

                public InsertCustomerCallback(InsertCustomerUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertCustomerResponse response)
                {
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
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

            public void OnSuccess(InsertCustomerResponse response)
            {
            }

            public void OnFailure(ZBankError response)
            {

            }
        }
}
