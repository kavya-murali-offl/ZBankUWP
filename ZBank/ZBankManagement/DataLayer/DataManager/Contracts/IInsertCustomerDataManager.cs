using ZBankManagement.Domain.UseCase;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IInsertCustomerDataManager
    {
        void InsertCustomer(InsertCustomerRequest request, IUseCaseCallback<InsertCustomerResponse> callback);
    }
}
