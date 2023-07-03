using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.Entity.EnumerationTypes;
using System.Threading.Tasks;
using System;

namespace ZBankManagement.DataManager
{
    class UpdateCardDataManager : IUpdateCardDataManager
    {

        public UpdateCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task UpdateCard(UpdateCardRequest request, IUseCaseCallback<UpdateCardResponse> callback)
        {
            try
            {
                int rowsModified = await DBHandler.UpdateCard(request.CardToUpdate.CardNumber, request.CardToUpdate.TransactionLimit, request.CustomerID);
                if (rowsModified > 0)
                {
                    UpdateCardResponse response = new UpdateCardResponse();
                    response.UpdatedCard = request.CardToUpdate;
                    response.IsSuccess = true;
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Error in fetching data";
                    error.Type = ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }
            }catch(Exception ex)
            {
                ZBankException error = new ZBankException();
                error.Message = ex.Message;
                error.Type = ErrorType.UNKNOWN;
                callback.OnFailure(error);
            }


        }

    }
}
