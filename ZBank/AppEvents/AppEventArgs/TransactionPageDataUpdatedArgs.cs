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
    public class TransactionPageDataUpdatedArgs
    {
        public IEnumerable<TransactionBObj> TransactionList { get; set; }

        public IEnumerable<Beneficiary> BeneficiariesList { get; set; }

    }
}
