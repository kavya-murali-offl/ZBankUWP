using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.EnumerationTypes;

namespace ZBankManagement.Domain.UseCase
{
    public class ZBankException
    {
        public ErrorType Type { get; set; }

        public string Message { get; set; }
    }
}
