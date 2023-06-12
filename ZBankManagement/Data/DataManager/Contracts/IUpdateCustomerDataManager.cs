using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    public interface IUpdateCustomerDataManager
    {
        Task UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback);

        Task LogoutCustomer(LogoutCustomerRequest request, IUseCaseCallback<LogoutCustomerResponse> callback);
    }
}
