using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    interface IUpdateAccountDataManager
    {
        Task UpdateAccount(UpdateAccountRequest request, IUseCaseCallback<UpdateAccountResponse> callback);
    }
}