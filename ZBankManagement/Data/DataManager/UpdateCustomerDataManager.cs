using ZBankManagement.Interface;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System;
using System.Threading.Tasks;
using ZBank.Entity.EnumerationTypes;
using System.Collections;
using ZBank.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ZBankManagement.DataManager
{
    class UpdateCustomerDataManager : IUpdateCustomerDataManager
    {
        public UpdateCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback)
        {
            try
            {
                int rowsModified = await DBHandler.UpdateCustomer(request.CustomerToUpdate).ConfigureAwait(false);
                if (rowsModified > 0)
                {
                    UpdateCustomerResponse response = new UpdateCustomerResponse
                    {
                        InsertedAccount = request.CustomerToUpdate
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Customer not updated";
                    error.Type = ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }
            }
            catch (Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }

        public async Task UpdateCustomer(LogoutCustomerRequest request, IUseCaseCallback<LogoutCustomerResponse> callback)
        {
            try
            {
                IEnumerable<Customer> customers = await DBHandler.GetCustomer(request.CustomerID).ConfigureAwait(false);
                if (customers.Count() > 0)
                {
                    var customer = customers.First();
                    customer.LastLoggedOn = DateTime.Now;
                    LogoutCustomerResponse response = new LogoutCustomerResponse
                    {
                         UpdatedCustomer = customer,
                    };
                    callback.OnSuccess(response);
                }
                else
                {
                    ZBankException error = new ZBankException();
                    error.Message = "Customer not found";
                    error.Type = ErrorType.UNKNOWN;
                    callback.OnFailure(error);
                }
            }
            catch (Exception err)
            {
                ZBankException error = new ZBankException();
                error.Message = err.Message;
                error.Type = ErrorType.DATABASE_ERROR;
                callback.OnFailure(error);
            }
        }
       
    }
}
