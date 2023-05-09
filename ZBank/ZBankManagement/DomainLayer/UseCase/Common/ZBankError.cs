using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.EnumerationTypes;

namespace BankManagementDB.Domain.UseCase
{
    public class ZBankError
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
    }

}
