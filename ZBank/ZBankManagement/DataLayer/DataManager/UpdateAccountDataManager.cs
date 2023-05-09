using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateAccount;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.DataManager
{
    public  class UpdateAccountDataManager : IUpdateAccountDataManager
    {
        public UpdateAccountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }


        public void UpdateAccount<T>(UpdateAccountRequest<T> request, IUseCaseCallback<UpdateAccountResponse<T>> callback)
        {
            bool result = DBHandler.UpdateAccount(request.UpdatedAccount).Result;

            UpdateAccountResponse<T> response = new UpdateAccountResponse<T>();
            response.IsSuccess = result;

            if(result)
            {
                response.UpdatedAccount = request.UpdatedAccount;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankError error = new ZBankError();
                error.Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN;
                callback.OnFailure(error);  
            }
        }
    }
}
