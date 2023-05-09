using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.InsertCard;

namespace BankManagementDB.DataManager
{
    public interface IInsertCardDataManager
    {
        void InsertCard(InsertCardRequest request, IUseCaseCallback<InsertCardResponse> callback);
    }
}