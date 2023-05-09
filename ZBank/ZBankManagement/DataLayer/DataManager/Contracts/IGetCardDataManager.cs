using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.Interface
{
    public interface IGetCardDataManager
    {
        void GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback);
    }
}
