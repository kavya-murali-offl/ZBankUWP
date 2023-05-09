using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertCustomer;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.Interface
{
    public interface IInsertCustomerDataManager
    {
        void InsertCustomer(InsertCustomerRequest request, IUseCaseCallback<InsertCustomerResponse> callback);
    }
}
