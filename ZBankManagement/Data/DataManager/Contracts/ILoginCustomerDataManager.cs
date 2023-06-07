using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace ZBankManagement.Interface
{
    public interface ILoginCustomerDataManager
    {
        void GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback);

        void GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback);

    }
}
