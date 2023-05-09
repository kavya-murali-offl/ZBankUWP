using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateAccount;

namespace BankManagementDB.Interface
{
    public interface IUpdateAccountDataManager
    {
        void UpdateAccount<T>(UpdateAccountRequest<T> request, IUseCaseCallback<UpdateAccountResponse<T>> callback);
    }
}