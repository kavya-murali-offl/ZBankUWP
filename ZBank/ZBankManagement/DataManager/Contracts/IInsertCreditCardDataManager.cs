using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities.BusinessObjects;

namespace BankManagementDB.Interface
{
    public interface IInsertCreditCardDataManager
    {
        bool InsertCreditCard(CreditCardDTO card);
    }
}
