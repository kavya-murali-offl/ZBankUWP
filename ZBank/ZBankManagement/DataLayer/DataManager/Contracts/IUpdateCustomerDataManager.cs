using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCustomer;

namespace BankManagementDB.Interface
{
    public interface IUpdateCustomerDataManager
    {
        bool UpdateCustomer(UpdateCustomerRequest request, IUseCaseCallback<UpdateCustomerResponse> callback);
    }
}
