using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBankManagement.DataManager
{
    public interface IInsertCardDataManager
    {
        void InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback);
    }
}