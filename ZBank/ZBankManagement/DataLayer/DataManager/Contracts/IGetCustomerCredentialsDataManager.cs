
using BankManagementDB.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace BankManagementDB.Interface
{
    public interface IGetCustomerCredentialsDataManager
    {
        void GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback);    
    }
}

