using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using BankManagementDB.Domain.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCustomer;

namespace BankManagementDB.DataManager
{
    public class UpdateCustomerDataManager : IUpdateCustomerDataManager
    {
        public UpdateCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback)
        {
            bool success = DBHandler.UpdateCustomer(request.CustomerToUpdate).Result;
            return success;
        }
    }
}
