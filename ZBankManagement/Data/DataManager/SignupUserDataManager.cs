using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.Entity.EnumerationTypes;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ZBankManagement.DataLayer.DataManager
{
    class SignupUserDataManager : ISignupUserDataManager
    {
        public SignupUserDataManager(IDBHandler dbHandler)
        {
            DBHandler = dbHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public async Task SignupUser(InsertCustomerRequest request, IUseCaseCallback<SignupUserResponse> callback)
        {
            try
            {
                await DBHandler.InsertCustomer(request.Customer, request.CustomerCredentials);

                SignupUserResponse response = new SignupUserResponse
                {
                    IsSuccess = true,
                    InsertedCustomer = request.Customer
                };
                callback.OnSuccess(response);
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
