using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Linq;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Notifications;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.Utility;
using System.Security.Principal;
using System.Text;
using Windows.Storage;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertAccountUseCase : UseCaseBase<InsertAccountResponse>
        {
            private readonly IInsertAccountDataManager _insertAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertAccountDataManager>();
            private readonly InsertAccountRequest _request;

            public InsertAccountUseCase(InsertAccountRequest request, IPresenterCallback<InsertAccountResponse> presenterCallback) : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _request.AccountToInsert.AccountNumber = GenerateAccountNumber();
                _insertAccountDataManager.InsertAccount(_request, new InsertAccountCallback(this));
            }


        public string GenerateAccountNumber()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 16; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
                if(i == 3 || i == 7 || i == 11) { builder.Append(" "); }
            }

            return builder.ToString();
        }
        private class InsertAccountCallback : IUseCaseCallback<InsertAccountResponse>
            {

                private InsertAccountUseCase _useCase;

                public InsertAccountCallback(InsertAccountUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertAccountResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertAccountRequest : RequestObjectBase
        {
            public Account AccountToInsert { get; set; }

            public string CustomerID { get; set; }

            public IReadOnlyList<StorageFile> Documents { get; set; }
        }

        public class InsertAccountResponse
        {
            public bool IsSuccess { get; set; }

            public Account InsertedAccount { get; set; }
        }


}
