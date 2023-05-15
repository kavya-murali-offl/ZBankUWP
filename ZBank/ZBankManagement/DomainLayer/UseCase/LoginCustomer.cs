using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;
using System.Security.Cryptography;
using System;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class LoginCustomerUseCase : UseCaseBase<LoginCustomerResponse>
    {
        private readonly ILoginCustomerDataManager _loginCustomerDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ILoginCustomerDataManager>();
        private readonly LoginCustomerRequest _request;

        public LoginCustomerUseCase(LoginCustomerRequest request, IPresenterCallback<LoginCustomerResponse> presenterCallback) 
        : base(presenterCallback, request.Token) { 
            _request = request;
        }

        protected override void Action()
        {
            GetCredentialsRequest credentialsRequest = new GetCredentialsRequest();
            credentialsRequest.CustomerID = _request.CustomerID;
            _loginCustomerDataManager.GetCredentials(credentialsRequest, new GetCustomerCredentialsCallback(this));
        }

        private bool CheckPassword(string hashedCorrectPassword, string salt)
        {
            return hashedCorrectPassword == HashPassword(_request.Password, salt);
        }

        private string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10101))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(70));
            }
        }

        private class GetCustomerCredentialsCallback : IUseCaseCallback<GetCredentialsResponse>
        {
            private readonly LoginCustomerUseCase _useCase;

            public GetCustomerCredentialsCallback(LoginCustomerUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetCredentialsResponse response)
            {
                if (_useCase.CheckPassword(response.CustomerCredentials.Password, response.CustomerCredentials.Salt))
                {
                    GetCustomerRequest getCustomerRequest = new GetCustomerRequest();
                    getCustomerRequest.CustomerID = response.CustomerCredentials.ID;
                    _useCase._loginCustomerDataManager.GetCustomer(getCustomerRequest, new GetCustomerCallback(_useCase));
                }
                else
                {
                    ZBankException error = new ZBankException
                    {
                        Message = "Invalid Credentials"
                    };
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }


        public class GetCustomerCallback : IUseCaseCallback<GetCustomerResponse>
        {
            private readonly LoginCustomerUseCase _useCase;

            public GetCustomerCallback(LoginCustomerUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetCustomerResponse response)
            {

                LoginCustomerResponse loginResponse = new LoginCustomerResponse();
                loginResponse.LoggedInCustomer = response.Customer;
                loginResponse.IsLoggedIn = true;
                _useCase.PresenterCallback.OnSuccess(loginResponse);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
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

    public class LoginCustomerRequest : RequestObjectBase
        { 
            public string CustomerID { get; set; }
            public string Password { get; set; }
        }


    public class LoginCustomerResponse
    {
        public bool IsLoggedIn { get; set; }
        public Customer LoggedInCustomer { get; set; }
    }
}
