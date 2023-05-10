using BankManagementDB.Domain.UseCase;
using BankManagementDB.Interface;
using ZBank.DatabaseHandler;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace BankManagementDB.DataManager
{
    public class GetCustomerCredentialsDataManager : IGetCustomerCredentialsDataManager
    {
        public GetCustomerCredentialsDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }
        private IDBHandler DBHandler { get; set; }

        public void GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback)
        {
            CustomerCredentials credentials = DBHandler.GetCredentials(request.CustomerID).Result;
            
        }
      
    }
}
