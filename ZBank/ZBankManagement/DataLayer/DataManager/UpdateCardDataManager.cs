﻿using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.Entity.EnumerationTypes;

namespace ZBankManagement.DataManager
{
    public class UpdateCardDataManager : IUpdateCardDataManager
    {

        public UpdateCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void UpdateCard(UpdateCardRequest request, IUseCaseCallback<UpdateCardResponse> callback)
        {
            int rowsModified = DBHandler.UpdateCard(request.UpdatedCard).Result;
            if(rowsModified > 0)
            {
                UpdateCardResponse response = new UpdateCardResponse();
                response.UpdatedCard = request.UpdatedCard;
                response.IsSuccess = true;
                callback.OnSuccess(response);   
            }
            else
            {
                ZBankError error = new ZBankError();
                error.Message = "Error in fetching data";
                error.Type = ErrorType.UNKNOWN;
            }

        }

    }
}
