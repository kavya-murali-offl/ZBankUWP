using ZBankManagement.Interface;
using ZBank.Entities;
using System.Linq;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    class LoginCustomerDataManager  : ILoginCustomerDataManager
    {
        public LoginCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public async Task GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback)
        {
            try{
                IEnumerable<Customer> customers = await DBHandler.GetCustomer(request.CustomerID).ConfigureAwait(false);
                Customer customer = customers.FirstOrDefault();
                if (customer != null)
                {
                    GetCustomerResponse response = new GetCustomerResponse()
                    {
                        Customer = customer,
                    };

                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException exception = new ZBankException()
                    {
                        Message = "Customer not found",
                        Type = ZBank.Entity.EnumerationTypes.ErrorType.UNKNOWN
                    };

                    callback.OnFailure(exception);
                }
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

        public async Task GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback)
        {
            try
            {
                CustomerCredentials credentials = await DBHandler.GetCredentials(request.CustomerID).ConfigureAwait(false);
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
