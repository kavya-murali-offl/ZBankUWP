using BankManagementDB.Interface;
using ZBank.Entities;
using System.Linq;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.DataManager
{
    public class GetCustomerDataManager  : IGetCustomerDataManager
    {
        public GetCustomerDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public void GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback)
        {
           Customer customer = DBHandler.GetCustomer(request.CustomerID).Result.FirstOrDefault();
        }
    }
}
