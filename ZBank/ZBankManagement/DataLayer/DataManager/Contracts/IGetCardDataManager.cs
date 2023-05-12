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
        void GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback);
    }
}
