using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Cryptography;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.Services;
using ZBank.ZBankManagement.Services.Contracts;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class SignupUserUseCase : UseCaseBase<SignupUserResponse>
    {
        private readonly ISignupUserDataManager _signupUserDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ISignupUserDataManager>();
        private readonly IPresenterCallback<SignupUserResponse> _presenterCallback;
        private readonly SignupUserRequest _request;
        private readonly IPasswordHasherService _passwordHasherService;

        public SignupUserUseCase(SignupUserRequest request, IPresenterCallback<SignupUserResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
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

            private SignupUserUseCase _useCase;

            public SignupUserCallback(SignupUserUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(SignupUserResponse response)
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
        public Customer Customer { get; set; }

        public CustomerCredentials CustomerCredentials { get; set; }
    }

    public class SignupUserRequest
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
        private AccountPageViewModel AccountPageViewModel { get; set; }

        public SignupUserPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
            AccountPageViewModel = accountPageViewModel;
        }

        public async void OnSuccess(SignupUserResponse response)
        {
            await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (AccountPageViewModel.Accounts != null)
                {
                    //var accounts = AccountPageViewModel.Accounts.Prepend(response.InsertedAccount);
                    //AccountPageViewModel.Accounts = new ObservableCollection<Account>(accounts);
                }
            });
        }

        public void OnFailure(ZBankError error)
        {

        }
    }
}
