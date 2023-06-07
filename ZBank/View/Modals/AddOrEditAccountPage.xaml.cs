using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;
using ZBank.ViewModel.VMObjects;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Modals
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddOrEditAccountPage : Page, IView
    {
        public AddOrEditAccountViewModel ViewModel { get; set; }


        public AddOrEditAccountPage()
        {
            this.InitializeComponent();
            ViewModel = new AddOrEditAccountViewModel(this);
        }


        private string OnSelection { get; set; }

        private void AccountTypeButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons radios)
            {
                string accountType = radios.SelectedItem as string;
                DataTemplate template = null;

                switch (accountType)
                {
                    case "Current":
                        template = Resources["CurrentAccountFormTemplate"] as DataTemplate;
                        break;
                    case "Savings":
                        template = Resources["SavingsAccountFormTemplate"] as DataTemplate;
                        break;
                    case "Deposit":
                        template = Resources["DepositAccountFormTemplate"] as DataTemplate;
                        break;
                }

                OnSelection = accountType;

                if (template != null)
                {
                    AccountForm.DataContext = ViewModel;
                    AccountForm.Content = template.LoadContent();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountForm.DataContext = ViewModel;
            DataTemplate template = Resources["CurrentAccountFormTemplate"] as DataTemplate;
            AccountForm.Content = template.LoadContent();
            ViewNotifier.Instance.ThemeChanged += ChangeTheme;
            ViewModel.LoadContent();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
            ViewModel.UnloadContent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            //switch (OnSelection)
            //{
            //    case "Deposit":
            //        TermDepositAccount filledDepositAccount = ViewModel.DepositAccount;
            //        if (filledDepositAccount.Balance > 0 && ViewModel.DepositAccount.RepaymentAccountNumber != null &&
            //            ViewModel.DepositAccount.RepaymentAccountNumber != string.Empty &&
            //            ViewModel.DepositAccount.TenureInMonths > 0)
            //        {
            //            ViewModel.ApplyNewAccount(ViewModel.DepositAccount);
            //        }
            //        break;

            //    case "Current";

            //}
        }

        private async void ChangeTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = RequestedTheme = theme;
            });
        }

        private void BranchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    
}
