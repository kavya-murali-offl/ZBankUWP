using BankManagementDB.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;

namespace BankManagementDB.Interface
{
    public interface IUpdateCardDataManager
    {
        void UpdateCard(UpdateCardRequest request, IUseCaseCallback<UpdateCardResponse> callback);
    }
}
