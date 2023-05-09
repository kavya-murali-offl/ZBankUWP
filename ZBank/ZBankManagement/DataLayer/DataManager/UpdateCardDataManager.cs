using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using BankManagementDB.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.Entity.EnumerationTypes;

namespace BankManagementDB.DataManager
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
            bool result = DBHandler.UpdateCard(request.UpdatedCard).Result;
            if(result)
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
