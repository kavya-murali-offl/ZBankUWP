using System.Collections;
using System.Collections.Generic;
using ZBank.Entities;

namespace ZBank.AppEvents.AppEventArgs
{
    public class BranchListUpdatedArgs
    {
        public IEnumerable<Branch> BranchList { get; set; }
    }
}