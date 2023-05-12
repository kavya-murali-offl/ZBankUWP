using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IUpdateAccountDataManager
    {
        void UpdateAccount(UpdateAccountRequest request, IUseCaseCallback<UpdateAccountResponse> callback);
    }
}