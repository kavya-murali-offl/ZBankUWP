using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    interface ILoginCustomerDataManager
    {
        Task GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback);

        Task GetCredentials(GetCredentialsRequest request, IUseCaseCallback<GetCredentialsResponse> callback);

    }
}
