using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.DataManager;
using ZBankManagement.Interface;

namespace ZBankManagement.Domain.UseCase
{
        public class GetCustomerUseCase : UseCaseBase<GetCustomerResponse>
        {

            private readonly ILoginCustomerDataManager loginCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ILoginCustomerDataManager>();
            private readonly GetCustomerRequest _request;

            public GetCustomerUseCase(GetCustomerRequest request, IPresenterCallback<GetCustomerResponse> presenterCallback)
                : base(presenterCallback, request.Token)
            {
                _request = request;
            }


            protected override void Action()
            {
                loginCustomerDataManager.GetCustomer(_request, new GetCustomerCallback(this));
            }

            private class GetCustomerCallback : IUseCaseCallback<GetCustomerResponse>
            {
                private readonly UseCaseBase<GetCustomerResponse> _useCase;

                public GetCustomerCallback(GetCustomerUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(GetCustomerResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }
}
