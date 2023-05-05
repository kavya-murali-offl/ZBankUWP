using BankManagementDB.Interface;
using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;
using ZBank.DatabaseHandler;

namespace BankManagementDB.DataManager
{
    public class UpdateCreditCardDataManager : IUpdateCreditCardDataManager
    {
        public UpdateCreditCardDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool UpdateCreditCard(CreditCardDTO updatedCard) => DBHandler.UpdateCreditCard(updatedCard).Result;
    }
}
