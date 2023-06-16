using ZBankManagement.Interface;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System;
using System.Threading.Tasks;

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
            int rowsModified = await DBHandler.UpdateCustomer(request.CustomerToUpdate);
        }

        public async Task LogoutCustomer(LogoutCustomerRequest request, IUseCaseCallback<LogoutCustomerResponse> callback)
        {
            int rowsModified = await DBHandler.UpdateCustomer(request.LoggedInCustomer);
        }
    }
}
