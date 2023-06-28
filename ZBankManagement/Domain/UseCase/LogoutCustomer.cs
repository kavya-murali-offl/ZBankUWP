using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class LogoutCustomerUseCase : UseCaseBase<LogoutCustomerResponse>
    {
        private readonly IUpdateCustomerDataManager _updateCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateCustomerDataManager>();
        private readonly LogoutCustomerRequest _request;

        public LogoutCustomerUseCase(LogoutCustomerRequest request, IPresenterCallback<LogoutCustomerResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;

        }
        protected override void Action()
        {
            _updateCustomerDataManager.UpdateCustomer(_request, new LogoutCustomerCallback(this));
        }

        private class LogoutCustomerCallback : IUseCaseCallback<LogoutCustomerResponse>
        {
            private readonly LogoutCustomerUseCase _useCase;

            public LogoutCustomerCallback(LogoutCustomerUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(LogoutCustomerResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class LogoutCustomerRequest : RequestObjectBase
    {
        public string CustomerID { get; set; }
    }

    public class LogoutCustomerResponse
    {
        public bool IsSuccess { get; set; }
        public Customer UpdatedCustomer { get; set; }
    }

}
