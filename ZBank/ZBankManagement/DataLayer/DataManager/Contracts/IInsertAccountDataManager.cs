using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IInsertAccountDataManager
    {
        void InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback);

    }
}
