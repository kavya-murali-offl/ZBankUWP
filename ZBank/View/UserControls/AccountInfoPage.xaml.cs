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
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountInfoPage : Page, IView
    {
        public AccountInfoViewModel ViewModel { get; set; } 
        
        public AccountInfoPage()
        {
            this.InitializeComponent();
            ViewModel = new AccountInfoViewModel(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.OnPageLoaded();
            if(e.Parameter is AccountInfoPageParams) { 
                var parameters = (e.Parameter) as AccountInfoPageParams;
                if(parameters.AccountNumber != null)
                {
                    ViewModel.LoadAccount(parameters.AccountNumber);
                }
                else
                {
                    UpdateSelectedAccount(parameters.SelectedAccount);
                }
            }
        }

        private void UpdateSelectedAccount(AccountBObj selectedAccount)
        {
            if(selectedAccount == null)
            {
                selectedAccount = ViewModel.SelectedAccount;
            }
            else
            {
                ViewModel.UpdateSelectedAccount(selectedAccount);
            }

            DataTemplate template = null;

            if (selectedAccount is SavingsAccount)
            {
                template = Resources["SavingsAccountTemplate"] as DataTemplate;
            }
            else if (selectedAccount is CurrentAccount)
            {
                template = Resources["CurrentAccountTemplate"] as DataTemplate;
            }
            else if (selectedAccount is TermDepositAccount)
            {
                template = Resources["DepositAccountTemplate"] as DataTemplate;
            }

            if (template != null)
            {
                AccountInfoContentControl.DataContext = ViewModel;
                AccountInfoContentControl.Content = template.LoadContent();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountUpdated += OnAccountUpdated;
            ViewModel.OnPageLoaded();
        }

        private void OnAccountUpdated(bool arg1, AccountBObj obj)
        {
            UpdateSelectedAccount(obj);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountUpdated -= OnAccountUpdated;
            ViewModel.OnPageUnLoaded();
        }
    }

    public class AccountInfoPageParams
    {
        public AccountBObj SelectedAccount { get; set; }

        public string AccountNumber { get; set; }
    }
}
