using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entity.EnumerationTypes
{
    public enum DepositType
    {
        Cumulative = 0,
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Annual = 4,
        OnMaturity =5

    }
}
