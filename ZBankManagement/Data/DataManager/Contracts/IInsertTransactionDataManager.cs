using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    public interface IInsertTransactionDataManager
    {
        Task InsertTransaction(InsertTransactionRequest request, IUseCaseCallback<InsertTransactionResponse> callback);
    }
}
