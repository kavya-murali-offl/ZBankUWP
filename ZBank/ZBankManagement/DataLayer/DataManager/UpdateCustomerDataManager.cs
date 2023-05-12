using ZBankManagement.Interface;
using ZBank.DatabaseHandler;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public class UpdateCustomerDataManager : IUpdateCustomerDataManager
    {
        public UpdateCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public void UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback)
        {
            int rowsModified = DBHandler.UpdateCustomer(request.CustomerToUpdate).Result;
        }
    }
}
