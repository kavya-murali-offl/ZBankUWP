using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertAccount;

namespace BankManagementDB.Interface
{
    public interface IInsertAccountDataManager
    {
        void InsertAccount(InsertAccountRequest request, IUseCaseCallback<InsertAccountResponse> callback);

    }
}
