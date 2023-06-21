using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZBankManagement.Entity.BusinessObjects
{
    public class StepModel
    {
        public int StepNumber { get; set; }
        public string PrimaryCommandText { get; set; }
        public string SecondaryCommandText { get; set; }
        public ICommand PrimaryCommand { get; set; }
        public ICommand SecondaryCommand { get; set; }
        public object Content { get; set; }
    }
}
