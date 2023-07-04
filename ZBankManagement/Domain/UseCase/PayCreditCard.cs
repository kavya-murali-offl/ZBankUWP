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
using ZBankManagement.Data.DataManager.Contracts;
using ZBankManagement.Entities.BusinessObjects;

namespace ZBankManagement.Domain.UseCase
{
    public class PayCreditCardUseCase : UseCaseBase<PayCreditCardResponse>
    {

        private readonly IPayCreditCardDataManager _payCreditCardDataManager;
        private readonly PayCreditCardRequest _request;

        public PayCreditCardUseCase(PayCreditCardRequest request, IPresenterCallback<PayCreditCardResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _payCreditCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IPayCreditCardDataManager>();
            _request = request;
        }


        protected override void Action()
        {
            _payCreditCardDataManager.PayCreditCard(_request, new PayCreditCardCallback(this));
        }

        private class PayCreditCardCallback : IUseCaseCallback<PayCreditCardResponse>
        {

            private readonly PayCreditCardUseCase _useCase;

            public PayCreditCardCallback(PayCreditCardUseCase useCase)
            {
                _useCase = useCase;
            }

            public void OnSuccess(PayCreditCardResponse response)
            {
                _useCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                _useCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class PayCreditCardRequest : RequestObjectBase
    {
        public CreditCard CreditCard { get; set; }
        public string CustomerID { get; set; }
        public Account PaymentAccount { get; set; }
        public decimal PaymentAmount { get; set; }
    }


    public class PayCreditCardResponse
    {
        public CreditCard CreditCard { get; set; }
        public Account PaymentAccount { get; set; }

    }
}
