using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.Constants;

namespace ZBank.Utilities.Helpers
{
    public class LogoHelper
    {

        public static string GetCardProviderPath(CreditCardProvider? provider=null)
        {
            switch(provider)
            {
                case CreditCardProvider.VISA:return Constants.VisaLogo;
                case CreditCardProvider.MASTERCARD: return Constants.MastercardLogo;
                case CreditCardProvider.RUPAY: return Constants.RupayLogo;
                default: return Constants.ZBankLogo;
            }
        }
    }
}
