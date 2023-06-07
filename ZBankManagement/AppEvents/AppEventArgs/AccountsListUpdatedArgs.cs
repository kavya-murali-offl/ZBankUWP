using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;

namespace ZBank.AppEvents.AppEventArgs
{
    public class AccountsListUpdatedArgs
    {
       public IEnumerable<Account> AccountsList;
    }
}
