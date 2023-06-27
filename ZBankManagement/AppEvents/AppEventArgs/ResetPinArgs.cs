using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBankManagement.AppEvents.AppEventArgs
{
    public class ResetPinArgs
    {
        public string CardNumber { get; set; }  
        public string PinNumber { get; set; }
    }
}
