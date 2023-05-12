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
using ZBank.ViewModel;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertBeneficiaryUseCase : UseCaseBase<InsertBeneficiaryResponse>
        {
            private readonly IInsertBeneficiaryDataManager _insertBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertBeneficiaryDataManager>();
            private readonly IPresenterCallback<InsertBeneficiaryResponse> _presenterCallback;
            private readonly InsertBeneficiaryRequest _request;

            public InsertBeneficiaryUseCase(InsertBeneficiaryRequest request, IPresenterCallback<InsertBeneficiaryResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
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
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertBeneficiaryRequest
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

            public void OnSuccess(InsertBeneficiaryResponse response)
            {
            }

            public void OnFailure(ZBankError error)
            {

            }
        }
}
