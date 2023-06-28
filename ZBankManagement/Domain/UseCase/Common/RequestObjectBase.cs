using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZBank.ZBankManagement.DomainLayer.UseCase.Common
{
    public abstract class RequestObjectBase
    {
        public CancellationToken Token;
    }
}
