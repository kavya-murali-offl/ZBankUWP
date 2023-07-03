using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class UpdateAccountUseCase : UseCaseBase<UpdateAccountResponse>
        {
            private readonly IUpdateAccountDataManager _updateAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateAccountDataManager>();
            private readonly UpdateAccountRequest _request;

            public UpdateAccountUseCase(UpdateAccountRequest request, IPresenterCallback<UpdateAccountResponse> presenterCallback)
            : base(presenterCallback, request.Token)
            {
                _request = request;

            }
            protected override void Action()
            {
                _updateAccountDataManager.UpdateAccount(_request, new UpdateAccountCallback(this));
            }

            private class UpdateAccountCallback : IUseCaseCallback<UpdateAccountResponse>
            {

                private readonly UpdateAccountUseCase _useCase;

                public UpdateAccountCallback(UpdateAccountUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(UpdateAccountResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class UpdateAccountRequest : RequestObjectBase
        {
            public Account UpdatedAccount { get; set; }
            public string CustomerID { get; set; }
        }

        public class UpdateAccountResponse
        {
            public bool IsSuccess { get; set; }
            public Account UpdatedAccount { get; set; }
        }

    }
