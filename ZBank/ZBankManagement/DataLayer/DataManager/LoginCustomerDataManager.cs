using ZBankManagement.Interface;
using ZBank.Entities;
using System.Linq;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System;

namespace ZBankManagement.DataManager
{
    public class LoginCustomerDataManager  : ILoginCustomerDataManager
    {
        public LoginCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public void GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback)
        {
            try{
                Customer customer = DBHandler.GetCustomer(request.CustomerID).Result.FirstOrDefault();

                GetCustomerResponse response = new GetCustomerResponse()
                {
                    Customer = customer,
                };

                callback.OnSuccess(response);
            }
            catch(Exception e)
            {
                ZBankException exception = new ZBankException()
                {
                    Message = e.Message,
                    Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN
                };

                callback.OnFailure(exception);
            }
        
        } 

        public void GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback)
        {
            try
            {
                CustomerCredentials credentials = DBHandler.GetCredentials(request.CustomerID).Result;
                GetCredentialsResponse response = new GetCredentialsResponse()
                {
                    CustomerCredentials = credentials,
                };

                callback.OnSuccess(response);

            }
            catch (Exception e)
            {
                ZBankException exception = new ZBankException() { Message = e.Message, Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN };
                callback.OnFailure(exception);
            }
        }
    }
}
