using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBankManagement.Domain.UseCase;

namespace ZBankManagement.AppEvents.AppEventArgs
{
    public class NotifyUserArgs
    {   
        public ZBankException Exception { get; set; }
    }
}
