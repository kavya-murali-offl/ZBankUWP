using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertCustomer;

namespace BankManagementDB.DataManager
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

            bool result = DBHandler.InsertCustomer(request.CustomerToInsert, customerCredentials).Result;

            if (result)
            {
                InsertCustomerResponse response = new InsertCustomerResponse();
                response.IsSuccess = result;
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
