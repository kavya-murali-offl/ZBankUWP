using ZBankManagement.Domain.UseCase;
using ZBank.Entities;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using System.Threading.Tasks;

namespace ZBankManagement.Interface
{
    interface IUpdateCardDataManager
    {
        Task UpdateCard(UpdateCardRequest request, IUseCaseCallback<UpdateCardResponse> callback);
    }
}
