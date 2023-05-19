using ZBankManagement.Data;
using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBank.Entities.BusinessObjects;

namespace ZBankManagement.DataManager
{
    public class GetCardDataManager : IGetCardDataManager
    {
        public GetCardDataManager(IDBHandler dBHandler)
        {
            _dBHandler = dBHandler;
        }

        private IDBHandler _dBHandler { get;  set; }

        public void GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            IEnumerable<Card> cards = _dBHandler.GetAllCards(request.CustomerID).Result;

            GetAllCardsResponse response = new GetAllCardsResponse();  
            callback.OnSuccess(response);
        }
    }
}
