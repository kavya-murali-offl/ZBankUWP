using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public class InsertCustomerDataManager : IInsertCustomerDataManager
    {
        public InsertCustomerDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void InsertCustomer(InsertCustomerRequest request, IUseCaseCallback<InsertCustomerResponse> callback){

            CustomerCredentials customerCredentials = new CustomerCredentials()
            {
                ID = request.CustomerToInsert.ID,
                Password = request.Password,
                Salt = ""
            };

            int rowsModified = DBHandler.InsertCustomer(request.CustomerToInsert, customerCredentials).Result;

            if (rowsModified > 0)
            {
                InsertCustomerResponse response = new InsertCustomerResponse();
                response.IsSuccess = rowsModified > 0;
                response.InsertedCustomer = request.CustomerToInsert;
                callback.OnSuccess(response);
            }
            else
            {
                ZBankError error = new ZBankError();
                error.Type = ErrorType.UNKNOWN;
                callback.OnFailure(error);
            }
       }
    }
}
