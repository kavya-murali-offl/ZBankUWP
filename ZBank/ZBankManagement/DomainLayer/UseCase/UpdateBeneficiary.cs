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

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class UpdateBeneficiaryUseCase : UseCaseBase<UpdateBeneficiaryResponse>
    {
        private readonly IUpdateBeneficiaryDataManager _updateBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IUpdateBeneficiaryDataManager>();
        private readonly IPresenterCallback<UpdateBeneficiaryResponse> _presenterCallback;
        private readonly UpdateBeneficiaryRequest _request;

        public UpdateBeneficiaryUseCase(UpdateBeneficiaryRequest request, IPresenterCallback<UpdateBeneficiaryResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
            _request = request;
        }

        protected override void Action()
        {
            _updateBeneficiaryDataManager.UpdateBeneficiary(_request, new UpdateBeneficiaryCallback(this));
        }

        private class UpdateBeneficiaryCallback : IUseCaseCallback<UpdateBeneficiaryResponse>
        {

            private UpdateBeneficiaryUseCase _useCase;

            public UpdateBeneficiaryCallback(UpdateBeneficiaryUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(UpdateBeneficiaryResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
            }
        }
    }

    public class UpdateBeneficiaryRequest
    {
        public Beneficiary BeneficiaryToUpdate { get; set; }
    }

    public class UpdateBeneficiaryResponse
    {
        public bool IsSuccess { get; set; }

        public Beneficiary UpdateedBeneficiary { get; set; }
    }

    public class UpdateBeneficiaryPresenterCallback : IPresenterCallback<UpdateBeneficiaryResponse>
    {


        public void OnSuccess(UpdateBeneficiaryResponse response)
        {
        }

        public void OnFailure(ZBankError error)
        {

        }
    }
}
