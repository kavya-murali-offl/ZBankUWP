using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.Entity.EnumerationTypes;
using System.Threading.Tasks;

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
            int rowsModified = await DBHandler.UpdateCard(request.UpdatedCard);
            if(rowsModified > 0)
            {
                UpdateCardResponse response = new UpdateCardResponse();
                response.UpdatedCard = request.UpdatedCard;
                response.IsSuccess = true;
                callback.OnSuccess(response);   
            }
            else
            {
                ZBankException error = new ZBankException();
                error.Message = "Error in fetching data";
                error.Type = ErrorType.UNKNOWN;
            }

        }

    }
}
