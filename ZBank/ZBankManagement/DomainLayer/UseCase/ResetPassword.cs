using Microsoft.Extensions.DependencyInjection;
using System;
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
    public class ResetPasswordUseCase : UseCaseBase<ResetPasswordResponse>
    {
        private readonly IResetPasswordDataManager _resetPasswordDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IResetPasswordDataManager>();
        private readonly ResetPasswordRequest _request;
        private readonly IPasswordHasherService _passwordHasherService;

        public ResetPasswordUseCase(ResetPasswordRequest request, IPresenterCallback<ResetPasswordResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _passwordHasherService = new PasswordHasherService();
            _request = request;

        }

        private CustomerCredentials GetUpdatedCredentialsObject()
        {
            string salt = _passwordHasherService.GenerateSalt();
            return new CustomerCredentials()
            {
                ID = _request.CustomerID,
                Password = _passwordHasherService.HashPassword(_request.PasswordToUpdate, salt),
                Salt = salt
            };
        }
        protected override void Action()
        {
            UpdateCustomerCredentialsRequest credentialsRequest = new UpdateCustomerCredentialsRequest();
            credentialsRequest.CustomerCredentials = GetUpdatedCredentialsObject();
            _resetPasswordDataManager.ResetPassword(credentialsRequest, new ResetPasswordCallback(this));
        }

        private class ResetPasswordCallback : IUseCaseCallback<ResetPasswordResponse>
        {
            private readonly ResetPasswordUseCase _useCase;

            public ResetPasswordCallback(ResetPasswordUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(ResetPasswordResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }

    }
    public class UpdateCustomerCredentialsRequest
    {
        public CustomerCredentials CustomerCredentials;
    }

    public class ResetPasswordRequest : RequestObjectBase
    {
        public string CustomerID { get; set; }

        public string PasswordToUpdate { get; set; }
    }

    public class ResetPasswordResponse
    {
        public bool IsSuccess { get; set; }
    }

    public class ResetPasswordPresenterCallback : IPresenterCallback<ResetPasswordResponse>
    {

        public ResetPasswordPresenterCallback(AccountPageViewModel accountPageViewModel)
        {
        }

        public async void OnSuccess(ResetPasswordResponse response)
        {
        }

        public void OnFailure(ZBankException response)
        {

        }
    }
}
