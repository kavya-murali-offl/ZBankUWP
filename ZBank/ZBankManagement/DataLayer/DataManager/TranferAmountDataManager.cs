using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using ZBank.DatabaseHandler;

namespace ZBankManagement.DataManager
{
    public class TranferAmountDataManager : ITransferAmountDataManager
    {
        public TranferAmountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }

        public bool PerformTransaction(IList<Action> actions) => false;
    }
}
