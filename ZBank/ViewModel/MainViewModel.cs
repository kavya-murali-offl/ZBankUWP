using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using ZBank.Config;
using ZBank.ViewModel.VMObjects;

namespace ZBank.ViewModel
{
    public class MainViewModel
    {
        public ICommand SwitchThemeCommand { get; private set; }


        public MainViewModel()
        {
        }

       
    }
}
