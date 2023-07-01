using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using ZBankManagement.DataManager;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entity.EnumerationTypes;
using ZBankManagement.Interface;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class TransferAmountUseCase : UseCaseBase<TransferAmountResponse>
    {
        private readonly ITransferAmountDataManager _transferAmountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<ITransferAmountDataManager>();
        private readonly TransferAmountRequest _request;

        public TransferAmountUseCase(TransferAmountRequest request, IPresenterCallback<TransferAmountResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }

        protected override void Action()
        {
           if(_request.Beneficiary != null)
           {
                if (_request.Beneficiary.BeneficiaryType == BeneficiaryType.WITHIN_BANK)
                {
                    _transferAmountDataManager.GetBeneficiaryAccount(_request, new GetBeneficiaryAccountCallback(this));
                }
                else if (_request.Beneficiary.BeneficiaryType == BeneficiaryType.OTHER_BANK)
                {
                    MakeExternalTransaction();
                }
            }
            else if(_request.OtherAccount != null)
            {
                _transferAmountDataManager.InitiateWithinBankTransaction(_request, new TransferAmountCallback(this));   
            }
           
        }

        private void MakeExternalTransaction()
        {
            _transferAmountDataManager.InitiateOtherBankTransaction(_request, new TransferAmountCallback(this));
        }

        private void MakeInternalTransaction(Account beneficiaryAccount)
        {
            if(beneficiaryAccount != null)
            {
                _transferAmountDataManager.InitiateWithinBankTransaction(_request, new TransferAmountCallback(this));
            }
        }

        private class GetBeneficiaryAccountCallback : IUseCaseCallback<GetBeneficiaryAccountResponse>
        {
            private readonly TransferAmountUseCase _useCase;

            public GetBeneficiaryAccountCallback(TransferAmountUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(GetBeneficiaryAccountResponse response)
            {
                if (response.Account != null)
                {
                    _useCase.MakeInternalTransaction(response.Account);
                }
                else
                {
                    ZBankException error = new ZBankException
                    {
                        Message = "Account Not Found"
                    };
                    _useCase.PresenterCallback.OnFailure(error);
                }
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }

        private class TransferAmountCallback : IUseCaseCallback<TransferAmountResponse>
        {
            private readonly TransferAmountUseCase UseCase;

            public TransferAmountCallback(TransferAmountUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(TransferAmountResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                UseCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class TransferAmountRequest : RequestObjectBase
    {
        public Transaction Transaction { get; set; }

        public Account OwnerAccount { get; set; }

        public Beneficiary Beneficiary { get; set; }  

        public Account OtherAccount { get; set; }   
    }


    public class TransferAmountResponse
    {
        public Transaction Transaction { get; set; }
    }


    public class GetBeneficiaryAccountResponse
    {
        public Account Account { get; set; }
    }

}
