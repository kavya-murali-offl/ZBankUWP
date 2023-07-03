using ZBankManagement.Domain.UseCase;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ZBank.Entity.EnumerationTypes;
using ZBankManagement.Entity.DTOs;
using ZBank.Entities.BusinessObjects;

namespace ZBankManagement.DataManager
{
    class InsertCardDataManager : IInsertCardDataManager
    {
        public InsertCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback)
        {

            try
            {
                if(request.CardToInsert is CreditCard)
                {
                    CreditCard creditCard = request.CardToInsert as CreditCard;
                    var creditCardDTO = new CreditCardDTO()
                    {
                        CardNumber = request.CardToInsert.CardNumber,
                        TotalOutstanding = creditCard.TotalOutstanding,
                        MinimumOutstanding = creditCard.MinimumOutstanding,
                        CreditLimit = creditCard.CreditLimit,
                        Interest = creditCard.Interest,
                        Provider = creditCard.CreditCardProvider
                    };
                    await DBHandler.InsertCreditCard(request.CardToInsert, creditCardDTO);

                }
                else if(request.CardToInsert is DebitCard)
                {
                    DebitCard debitCard = request.CardToInsert as DebitCard;
                    var debitCardDTO = new DebitCardDTO()
                    {
                        AccountNumber = debitCard.AccountNumber,
                        CardNumber = debitCard.CardNumber,
                    };
                    await DBHandler.InsertDebitCard(request.CardToInsert, debitCardDTO);

                }
                InsertCardResponse response = new InsertCardResponse
                {
                    IsSuccess = true,
                    InsertedCard = request.CardToInsert
                };
                callback.OnSuccess(response);

            }
            catch (Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }
    }
}
