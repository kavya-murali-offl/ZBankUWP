using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
        }

        private async void SwitchTheme(object parameter)
        {
            {
                if (ThemeSelector.Theme == ElementTheme.Light)
                {
                    await ThemeSelector.SetTheme(ElementTheme.Dark);
                }
                else if (ThemeSelector.Theme == ElementTheme.Dark)
                {
                    await ThemeSelector.SetTheme(ElementTheme.Light);
                }
                else
                {
                    await ThemeSelector.SetTheme(ElementTheme.Default);
                }
            }
        }

    }
}
