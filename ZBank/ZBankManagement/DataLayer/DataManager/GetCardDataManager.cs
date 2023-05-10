using BankManagementDB.Data;
using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using BankManagementDB.Domain.UseCase;

namespace BankManagementDB.DataManager
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
