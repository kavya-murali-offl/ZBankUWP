using BankManagementDB.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
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
            _request.Customer.ID = GenerateCustomerID();
            _request.Customer.CreatedOn = DateTime.Now;
            InsertCustomerRequest request = new InsertCustomerRequest()
            {
                Customer = _request.Customer,
                CustomerCredentials = GenerateCredentialsForCustomer(),
            };
            _signupUserDataManager.SignupUser(request, new SignupUserCallback(this));
        }

        private string GenerateCustomerID()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
            }

            return builder.ToString();
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

        public Customer InsertedCustomer { get; set; }
    }

}
