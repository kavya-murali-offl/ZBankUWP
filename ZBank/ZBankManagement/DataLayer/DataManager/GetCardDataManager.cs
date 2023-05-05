using BankManagementDB.Data;
using BankManagementDB.Interface;
using ZBank.Entities;
using ZBank.DatabaseHandler;
using System.Collections.Generic;
using System.Linq;

namespace BankManagementDB.DataManager
{
    public class GetCardDataManager : IGetCardDataManager
    {
        public GetCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get;  set; }

        public void GetAllCards(string customerID)
        {
            IEnumerable<Card> cards = new List<Card>();
            var creditCardsList = DBHandler.GetCreditCardByCustomerID(customerID).Result;
            var debitCardsList = DBHandler.GetDebitCardByCustomerID(customerID).Result;
            cards = cards.Concat(creditCardsList);
            cards = cards.Concat(debitCardsList);
            Store.CardsList = cards;
        }

    }
}
