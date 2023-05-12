using ZBankManagement.Interface;
using ZBank.Entities;
using System.Linq;
using ZBank.DatabaseHandler;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;

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
           Customer customer = DBHandler.GetCustomer(request.CustomerID).Result.FirstOrDefault();
        }

        public void GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback)
        {
            CustomerCredentials credentials = DBHandler.GetCredentials(request.CustomerID).Result;
        }
    }
}
