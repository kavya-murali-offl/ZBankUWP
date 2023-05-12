using ZBankManagement.Data;
using ZBankManagement.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.DataManager
{
    public class GetCardDataManager : IGetCardDataManager
    {
        public GetCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public void GetAllCards(GetAllCardsRequest request, IUseCaseCallback<GetAllCardsResponse> callback)
        {
            IEnumerable<Card> cards = new List<Card>();

            GetAllCardsResponse response = new GetAllCardsResponse();   
            response.Cards = cards;
            callback.OnSuccess(response);
        }
    }
}
