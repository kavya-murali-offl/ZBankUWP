using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.ZBankManagement.Services;
using ZBank.ZBankManagement.Services.Contracts;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class SignupUserUseCase : UseCaseBase<SignupUserResponse>
    {
        private readonly ISignupUserDataManager _signupUserDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ISignupUserDataManager>();
        private readonly SignupUserRequest _request;
        private readonly IPasswordHasherService _passwordHasherService;

        public SignupUserUseCase(SignupUserRequest request, IPresenterCallback<SignupUserResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
            _passwordHasherService = new PasswordHasherService();
        }

        protected override void Action()
        {
            InsertCustomerRequest request = new InsertCustomerRequest()
            {
                Customer = _request.Customer,
                CustomerCredentials = GenerateCredentialsForCustomer(),
            };
            _signupUserDataManager.SignupUser(request, new SignupUserCallback(this));
        }

        private CustomerCredentials GenerateCredentialsForCustomer()
        {
            string salt = _passwordHasherService.GenerateSalt();

            return new CustomerCredentials()
            {
                ID = _request.Customer.ID,
                Password = _passwordHasherService.HashPassword(_request.Password, salt),
                Salt = salt
            };
        }

        private class SignupUserCallback : IUseCaseCallback<SignupUserResponse>
        {

            private readonly SignupUserUseCase _useCase;

            public SignupUserCallback(SignupUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(SignupUserResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class InsertCustomerRequest 
    {
        public Customer Customer { get; set; }

        public CustomerCredentials CustomerCredentials { get; set; }
    }

    public class SignupUserRequest : RequestObjectBase
    {
        public Customer Customer { get; set; }

        public string Password { get; set; }
    }

    public class SignupUserResponse
    {
        public bool IsSuccess { get; set; }

        public Customer InsertedAccount { get; set; }
    }

    public class SignupUserPresenterCallback : IPresenterCallback<SignupUserResponse>
    {

        public SignupUserPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
        }

        public async void OnSuccess(SignupUserResponse response)
        {
        }

        public void OnFailure(ZBankException error)
        {
        }
    }
}
