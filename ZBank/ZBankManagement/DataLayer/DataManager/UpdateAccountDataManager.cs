using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public  class UpdateAccountDataManager : IUpdateAccountDataManager
    {
        public UpdateAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }


        public void UpdateAccount(UpdateAccountRequest request, IUseCaseCallback<UpdateAccountResponse> callback)
        {
            int rowsModified = DBHandler.UpdateAccount(request.UpdatedAccount).Result;

            if(rowsModified > 0)
            {
                UpdateAccountResponse response = new UpdateAccountResponse();
                response.IsSuccess = rowsModified > 0;
                response.UpdatedAccount = request.UpdatedAccount;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankException error = new ZBankException();
                error.Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN;
                callback.OnFailure(error);  
            }
        }
    }
}
