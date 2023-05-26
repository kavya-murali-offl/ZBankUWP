using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountInfoFrame : Page
    {
        public Account SelectedAccount { get; set; }    
        
        public AccountInfoFrame()
        {
            this.InitializeComponent(); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is AccountInfoPageParams) { 
                var parameters = (e.Parameter) as AccountInfoPageParams;
                SelectedAccount = parameters.SelectedAccount;
                DataTemplate template = null;

                if (SelectedAccount is SavingsAccount)
                {
                    template = Resources["SavingsAccountTemplate"] as DataTemplate;
                }
                else if (SelectedAccount is CurrentAccount)
                {
                    template = Resources["CurrentAccountTemplate"] as DataTemplate;
                }
                else if (SelectedAccount is TermDepositAccount)
                {
                    template = Resources["DepositAccountTemplate"] as DataTemplate;
                }

                if(template != null)
                {
                    AccountInfoContentControl.DataContext = this;
                    AccountInfoContentControl.Content = template.LoadContent();
                }
            }
        }
    }

    public class AccountInfoPageParams
    {
        public Account SelectedAccount { get; set; }    
    }
}
