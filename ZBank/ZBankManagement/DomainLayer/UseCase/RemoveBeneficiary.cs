using ZBankManagement.Domain.UseCase;
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

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class RemoveBeneficiaryUseCase : UseCaseBase<RemoveBeneficiaryResponse>
    {
        private readonly IDeleteBeneficiaryDataManager _deleteBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IDeleteBeneficiaryDataManager>();
        private readonly IPresenterCallback<RemoveBeneficiaryResponse> _presenterCallback;
        private readonly RemoveBeneficiaryRequest _request;

        public RemoveBeneficiaryUseCase(RemoveBeneficiaryRequest request, IPresenterCallback<RemoveBeneficiaryResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
            _request = request;
        }

        protected override void Action()
        {
            _deleteBeneficiaryDataManager.DeleteBeneficiary(_request, new RemoveBeneficiaryCallback(this));
        }

        private class RemoveBeneficiaryCallback : IUseCaseCallback<RemoveBeneficiaryResponse>
        {

            private RemoveBeneficiaryUseCase _useCase;

            public RemoveBeneficiaryCallback(RemoveBeneficiaryUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(RemoveBeneficiaryResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
            }
        }
    }

    public class RemoveBeneficiaryRequest
    {
        public Beneficiary BeneficiaryToRemove { get; set; }
    }

    public class RemoveBeneficiaryResponse
    {
        public bool IsSuccess { get; set; }

        public Beneficiary RemoveedBeneficiary { get; set; }
    }

    public class RemoveBeneficiaryPresenterCallback : IPresenterCallback<RemoveBeneficiaryResponse>
    {


        public void OnSuccess(RemoveBeneficiaryResponse response)
        {
        }

        public void OnFailure(ZBankError error)
        {

        }
    }
}
