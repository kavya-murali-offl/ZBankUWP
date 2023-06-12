﻿using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entities.BusinessObjects;
using System;
using ZBank.Entity.BusinessObjects;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    public class GetCardDataManager : IGetCardDataManager
    {
        public GetCardDataManager(IDBHandler dBHandler)
        {
            _dBHandler = dBHandler;
        }

        private IDBHandler _dBHandler { get; set; }

        public async Task GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            try
            {
                IEnumerable<CardBObj> cards = await _dBHandler.GetAllCards(request.CustomerID);

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

        public async Task GetCardByCardNumber(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            try
            {
                IEnumerable<CardBObj> cards = await _dBHandler.GetCardByCardNumber(request.CardNumber);

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
