using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.Interface
{
    public interface IGetCardDataManager
    {
        Task GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback);

        Task GetCardByCardNumber(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback);
    }
}
