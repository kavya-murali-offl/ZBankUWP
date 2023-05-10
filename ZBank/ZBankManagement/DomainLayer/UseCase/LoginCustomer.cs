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
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class LoginCustomerUseCase : UseCaseBase<LoginCustomerRequest, LoginCustomerResponse>
    {
        private readonly IGetCustomerCredentialsDataManager GetCustomerCredentialsDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetCustomerCredentialsDataManager>();

        private IPresenterCallback<LoginCustomerResponse> PresenterCallback;
        private LoginCustomerRequest Request;

        protected override void Action(LoginCustomerRequest request, IPresenterCallback<LoginCustomerResponse> presenterCallback)
        {
            PresenterCallback = presenterCallback;
            Request = request;

            GetCredentialsRequest credentialsRequest = new GetCredentialsRequest();
            credentialsRequest.CustomerID = request.CustomerID;
            GetCustomerCredentialsDataManager.GetCredentials(credentialsRequest, new GetCustomerCredentialsCallback(this));
        }

        public class GetCredentialsRequest
        {
            public string CustomerID { get; set; }
        }

        public class GetCustomerRequest
        {
            public string CustomerID { get; set; }
        }


        public class GetCredentialsResponse
        {
            public CustomerCredentials CustomerCredentials { get; set; }
        }

        public class GetCustomerResponse
        {
            public Customer Customer { get; set; }
        }


        private class GetCustomerCredentialsCallback : IUseCaseCallback<GetCredentialsResponse>
        {
            private readonly LoginCustomerUseCase UseCase;

            public GetCustomerCredentialsCallback(LoginCustomerUseCase useCase)
            {
                UseCase = useCase;
            }

            private readonly IGetCustomerDataManager GetCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetCustomerDataManager>();

            public void OnSuccess(GetCredentialsResponse response)
            {
                if(response.CustomerCredentials.Password == UseCase.Request.Password)
                {
                    GetCustomerRequest getCustomerRequest = new GetCustomerRequest();
                    getCustomerRequest.CustomerID = UseCase.Request.CustomerID;
                    GetCustomerDataManager.GetCustomer(getCustomerRequest, new GetCustomerCallback(UseCase));
                }
            }

            public void OnFailure(ZBankError error)
            {
                UseCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class GetCustomerCallback : IUseCaseCallback<GetCustomerResponse>
    {
        public LoginCustomerUseCase UseCase;

        public GetCustomerCallback(LoginCustomerUseCase useCase) { 
                UseCase = useCase;  
        }

        public void OnSuccess(GetCustomerResponse response)
        {
            //UseCase..OnSuccess(response);  
        }

        public void OnFailure(ZBankError error)
        {

        }
    }

    public class LoginCustomerRequest
    {
        public string CustomerID { get; set; }
        public string Password { get; set; }

    }

    public class LoginCustomerResponse
    {
        public bool IsLoggedIn { get; set; }

        public Customer LoginedCustomer { get; set; }

        public class LoginCustomerPresenterCallback : IPresenterCallback<LoginCustomerResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public LoginCustomerPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public void OnSuccess(LoginCustomerResponse response)
            {

            }

            public void OnFailure(ZBankError response)
            {

            }
        }
    }
}
