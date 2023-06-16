using ZBankManagement.Interface;
using System;
using System.Collections.Generic;
using ZBank.DatabaseHandler;

namespace ZBankManagement.DataManager
{
    class TranferAmountDataManager : ITransferAmountDataManager
    {
        public TranferAmountDataManager(IDBHandler dBHandler)
        {
            DBHandler = dBHandler;
        }

        private IDBHandler DBHandler { get; set; }


        public void UpdateAccountBalance()
        {

        }
    }
}
