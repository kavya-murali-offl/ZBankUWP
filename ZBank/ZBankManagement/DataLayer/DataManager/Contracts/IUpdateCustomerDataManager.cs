using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IUpdateCustomerDataManager
    {
        void UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback);
    }
}
