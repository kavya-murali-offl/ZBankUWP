using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.LoginCustomerUseCase;

namespace BankManagementDB.Interface
{
    public interface IGetCustomerDataManager
    {
        void GetCustomer(GetCustomerRequest request, IUseCaseCallback<GetCustomerResponse> callback);

    }
}
