using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertTransaction;

namespace BankManagementDB.Interface
{
    public interface IInsertTransactionDataManager
    {
        void InsertTransaction(InsertTransactionRequest request, IUseCaseCallback<InsertTransactionResponse> callback);
    }
}
