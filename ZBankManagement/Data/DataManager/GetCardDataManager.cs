using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entities.BusinessObjects;
using System;
using ZBank.Entity.BusinessObjects;

namespace ZBankManagement.DataManager
{
    public class GetCardDataManager : IGetCardDataManager
    {
        public GetCardDataManager(IDBHandler dBHandler)
        {
            _dBHandler = dBHandler;
        }

        private IDBHandler _dBHandler { get; set; }

        public void GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            try
            {
                IEnumerable<CardBObj> cards = _dBHandler.GetAllCards(request.CustomerID).Result;

                GetAllCardsResponse response = new GetAllCardsResponse();
                response.Cards = cards;
                callback.OnSuccess(response);
            }
            catch(Exception ex) {
                ZBankException exception = new ZBankException()
                {
                    Message = ex.Message,
                    Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN
                };

                callback.OnFailure(exception);  
            }
        }

        public void GetCardByCardNumber(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            try
            {
                IEnumerable<CardBObj> cards = _dBHandler.GetCardByCardNumber(request.CardNumber).Result;

                GetAllCardsResponse response = new GetAllCardsResponse();
                response.Cards = cards;
                callback.OnSuccess(response);
            }
            catch (Exception ex)
            {
                ZBankException exception = new ZBankException()
                {
                    Message = ex.Message,
                    Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN
                };

                callback.OnFailure(exception);
            }
        }
    }
}
