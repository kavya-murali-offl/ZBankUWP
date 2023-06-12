using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertBeneficiaryUseCase : UseCaseBase<InsertBeneficiaryResponse>
        {
            private readonly IInsertBeneficiaryDataManager _insertBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertBeneficiaryDataManager>();
            private readonly InsertBeneficiaryRequest _request;

            public InsertBeneficiaryUseCase(InsertBeneficiaryRequest request, IPresenterCallback<InsertBeneficiaryResponse> presenterCallback) 
            : base(presenterCallback, request.Token)
            {
                _request = request;
            }

            protected override void Action()
            {
                _insertBeneficiaryDataManager.InsertBeneficiary(_request, new InsertBeneficiaryCallback(this));
            }

            private class InsertBeneficiaryCallback : IUseCaseCallback<InsertBeneficiaryResponse>
            {

                private InsertBeneficiaryUseCase _useCase;

                public InsertBeneficiaryCallback(InsertBeneficiaryUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertBeneficiaryResponse response)
                {
                    _useCase.PresenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankException error)
                {
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertBeneficiaryRequest : RequestObjectBase
        {
            public Beneficiary BeneficiaryToInsert { get; set; }
        }

        public class InsertBeneficiaryResponse
        {
            public bool IsSuccess { get; set; }

            public Beneficiary InsertedBeneficiary { get; set; }
        }

        public class InsertBeneficiaryPresenterCallback : IPresenterCallback<InsertBeneficiaryResponse>
        {

            public async Task OnSuccess(InsertBeneficiaryResponse response)
            {
            }

            public async Task OnFailure(ZBankException error)
            {

            }
        }
}
