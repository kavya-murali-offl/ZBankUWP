using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
using ZBank.Config;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entity.BusinessObjects;
using ZBank.Services;
using ZBank.View.Modals;
using ZBank.View.UserControls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class DepositAccountInfoTemplate : UserControl, IView
    {
        public DepositAccountInfoTemplate()
        {
            this.InitializeComponent();
        }

        private bool IsActive { get => SelectedAccount.AccountStatus == AccountStatus.ACTIVE; }

        public ICommand CloseAccountCommand
        {
            get { return (ICommand)GetValue(CloseAccountCommandProperty); }
            set { SetValue(CloseAccountCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseAccountCommandProperty =
            DependencyProperty.Register("CloseAccountCommand", typeof(ICommand), typeof(DepositAccountInfoTemplate), new PropertyMetadata(null));


        public TermDepositAccount SelectedAccount
        {
            get { return (TermDepositAccount)GetValue(SelectedAccountProperty); }
            set { SetValue(SelectedAccountProperty, value); }
        }

        public static readonly DependencyProperty SelectedAccountProperty =
            DependencyProperty.Register("SelectedAccount", typeof(TermDepositAccount), typeof(DepositAccountInfoTemplate), new PropertyMetadata(null));


        public ICommand UpdateAccountCommand
        {
            get { return (ICommand)GetValue(UpdateAccountCommandProperty); }
            set { SetValue(UpdateAccountCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateAccountCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateAccountCommandProperty =
            DependencyProperty.Register("UpdateAccountCommand", typeof(ICommand), typeof(DepositAccountInfoTemplate), new PropertyMetadata(null));


        private async void AddBeneficiary_Click(object sender, RoutedEventArgs e)
        {
            await DialogService.ShowContentAsync(this, new AddEditBeneficiaryView(), "Add Beneficiary", this.XamlRoot);
        }

        private void CloseAccountButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAccountCommand.Execute(SelectedAccount);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RepaymentAccountNumberText.Text) || string.IsNullOrWhiteSpace(RepaymentAccountNumberText.Text))
            {
                UpdateErrorText.Text = "Field should not be empty";
            }
            else if(SelectedAccount?.RepaymentAccountNumber == RepaymentAccountNumberText.Text)
            {
                UpdateErrorText.Text = "Enter a different account number";
            }
            else
            {
                var updatedAcccount = new TermDepositAccount()
                {
                    AccountNumber = SelectedAccount.AccountNumber,
                    RepaymentAccountNumber = RepaymentAccountNumberText.Text,
                };
                UpdateAccountCommand.Execute(updatedAcccount);
            }
        }

        private void ResetUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            RepaymentAccountNumberText.Text = SelectedAccount.RepaymentAccountNumber;
        }

        private void RepaymentAccountNumberText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateErrorText.Text = "";
        }
    }
}
