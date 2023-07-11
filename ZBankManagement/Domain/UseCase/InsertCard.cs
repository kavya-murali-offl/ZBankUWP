using ZBankManagement.DataManager;
using ZBankManagement.Domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase.Common;
using System.Threading.Tasks;
using System.Text;
using System;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
    public class InsertCardUseCase : UseCaseBase<InsertCardResponse>
    {
        private readonly Random random = new Random();
        private readonly IInsertCardDataManager _insertCardDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertCardDataManager>();
        private readonly InsertCardRequest _request;

        public InsertCardUseCase(InsertCardRequest request, IPresenterCallback<InsertCardResponse> presenterCallback)
            : base(presenterCallback, request.Token)
        {
            _request = request;
        }

        protected override void Action()
        {
            _request.CardToInsert = new Card()
            {
                CardNumber = GenerateCardNumber(),
                CustomerID = _request.CustomerID,
                CVV = GenerateCVV(),
                Pin = GeneratePin(),
                ExpiryMonth = DateTime.Now.Month.ToString(),
                ExpiryYear = (DateTime.Now.Year + 8).ToString(),
                LinkedOn = DateTime.Now,
                TransactionLimit = 30000,
                Type = _request.CardType
            };
            _insertCardDataManager.InsertCard(_request, new InsertCardCallback(this));
        }

        private string GenerateCardNumber()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 16; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);

                if (i == 3 || i == 7 || i == 11)
                {
                    builder.Append(" ");
                }
            }

            return builder.ToString();
        }

       private string GeneratePin()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
            }

            return builder.ToString();
        }

       private string GenerateCVV()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 3; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
            }

            return builder.ToString();
        }

        private class InsertCardCallback : IUseCaseCallback<InsertCardResponse>
        {
            private readonly InsertCardUseCase UseCase;

            public InsertCardCallback(InsertCardUseCase useCase)
            {
                UseCase = useCase;
            }

            public void OnSuccess(InsertCardResponse response)
            {
                UseCase.PresenterCallback.OnSuccess(response);
            }

            public void OnFailure(ZBankException error)
            {
                UseCase.PresenterCallback.OnFailure(error);
            }
        }
    }

    public class InsertCardRequest : RequestObjectBase
    {
        public CreditCardProvider CreditCardProvider { get; set; }

        public CardType CardType { get; set; }

        public string CustomerID { get; set; }

        public string AccountNumber { get; set; }
        public Card CardToInsert { get; set; }

    }

    public class InsertCardResponse
    {
        public bool IsSuccess { get; set; }

        public Card InsertedCard { get; set; }
    }

}
