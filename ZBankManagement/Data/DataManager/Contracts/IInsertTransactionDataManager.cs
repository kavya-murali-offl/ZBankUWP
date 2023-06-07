using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.Interface
{
    public interface IInsertTransactionDataManager
    {
        void InsertTransaction(InsertTransactionRequest request, IUseCaseCallback<InsertTransactionResponse> callback);
    }
}
