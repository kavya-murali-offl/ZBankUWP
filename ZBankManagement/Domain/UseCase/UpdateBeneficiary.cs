using ZBankManagement.Domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class UpdateBeneficiaryUseCase : UseCaseBase<UpdateBeneficiaryResponse>
    {
        private readonly IUpdateBeneficiaryDataManager _updateBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateBeneficiaryDataManager>();
        private readonly UpdateBeneficiaryRequest _request;

        public UpdateBeneficiaryUseCase(UpdateBeneficiaryRequest request, IPresenterCallback<UpdateBeneficiaryResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }

        protected override void Action()
        {
            _updateBeneficiaryDataManager.UpdateBeneficiary(_request, new UpdateBeneficiaryCallback(this));
        }

        private class UpdateBeneficiaryCallback : IUseCaseCallback<UpdateBeneficiaryResponse>
        {

            private readonly UpdateBeneficiaryUseCase _useCase;

            public UpdateBeneficiaryCallback(UpdateBeneficiaryUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(UpdateBeneficiaryResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class UpdateBeneficiaryRequest : RequestObjectBase
    {
        public Beneficiary BeneficiaryToUpdate { get; set; }
    }

    public class UpdateBeneficiaryResponse
    {
        public bool IsSuccess { get; set; }

        public Beneficiary UpdatedBeneficiary { get; set; }
    }

}
