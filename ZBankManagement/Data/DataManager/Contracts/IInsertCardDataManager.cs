using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using System.Threading.Tasks;

namespace ZBankManagement.DataManager
{
    interface IInsertCardDataManager
    {
        Task InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback);
    }
}