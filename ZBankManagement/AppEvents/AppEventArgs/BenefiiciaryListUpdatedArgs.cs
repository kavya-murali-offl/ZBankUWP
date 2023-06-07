using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;

namespace ZBank.AppEvents.AppEventArgs
{
    public class BeneficiaryListUpdatedArgs
    {
        public IEnumerable<Beneficiary> BeneficiaryList;
    }
}
