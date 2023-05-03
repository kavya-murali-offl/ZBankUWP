using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.ViewModel
{
    public class TermDepositViewModel : ViewModelBase
    {
        private ObservableCollection<TermDepositAccount> _termDepositAccounts { get; set; } = new ObservableCollection<TermDepositAccount>();

        public TermDepositViewModel()
        {
        }


    }
}
