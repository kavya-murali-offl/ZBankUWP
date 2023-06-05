using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Utilities.Validation
{
    public interface IValidation
    {
        IDictionary<string, object> FieldValues { get; set; }
        IDictionary<string, object> FieldErrors { get; set; }
    }
}
