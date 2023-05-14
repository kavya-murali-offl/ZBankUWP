using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class GetAllBeneficiariesUseCase : UseCaseBase<GetAllBeneficiariesResponse>
    {

        private readonly IGetBeneficiaryDataManager _getBeneficiaryDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IGetBeneficiaryDataManager>();
        private readonly IPresenterCallback<GetAllBeneficiariesResponse> _presenterCallback;
        private readonly GetAllBeneficiariesRequest _request;

        public GetAllBeneficiariesUseCase(GetAllBeneficiariesRequest request, IPresenterCallback<GetAllBeneficiariesResponse> presenterCallback)
        {
            _presenterCallback = presenterCallback;
            _request = request;
        }


        protected override void Action()
        {
            _getBeneficiaryDataManager.GetAllBeneficiariesByUserID(_request, new GetAllBeneficiariesCallback(this));
        }

        private class GetAllBeneficiariesCallback : IUseCaseCallback<GetAllBeneficiariesResponse>
        {

            private readonly GetAllBeneficiariesUseCase _useCase;

            public GetAllBeneficiariesCallback(GetAllBeneficiariesUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetAllBeneficiariesResponse response)
            {
                _useCase._presenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankError error)
            {
                _useCase._presenterCallback.OnFailure(error);
            }
        }
    }

    public class GetAllBeneficiariesRequest
    {
        public string UserID { get; set; }
    }


    public class GetAllBeneficiariesResponse
    {
        public IEnumerable<Beneficiary> Beneficiaries { get; set; }
    }


    public class GetAllBeneficiariesPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {


        public void OnSuccess(GetAllBeneficiariesResponse response)
        {
        }

        public void OnFailure(ZBankError response)
        {
            // Notify view
        }
    }
}
