﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.ZBankManagement.AppEvents.AppEventArgs
{
    public class AccountsListUpdatedArgs
    {
       public ObservableCollection<Account> AccountsList;
    }
}
