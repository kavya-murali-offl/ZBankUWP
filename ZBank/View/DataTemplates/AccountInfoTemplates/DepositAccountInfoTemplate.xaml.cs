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
using ZBank.View.UserControls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class DepositAccountInfoTemplate : UserControl
    {
        public DepositAccountInfoTemplate()
        {
            this.InitializeComponent();
        }

     

        public TermDepositAccount CalculatorAccount
        {
            get { return (TermDepositAccount)GetValue(CalculatorAccountProperty); }
            set { SetValue(CalculatorAccountProperty, value); }
        }

        public static readonly DependencyProperty CalculatorAccountProperty =
            DependencyProperty.Register("CalculatorAccount", typeof(TermDepositAccount), typeof(DepositAccountInfoTemplate), new PropertyMetadata(new TermDepositAccount()));


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
            DependencyProperty.Register("SelectedAccount", typeof(TermDepositAccount), typeof(DepositAccountInfoTemplate), new PropertyMetadata(null, OnSelectedAccountChanged));

        private static void OnSelectedAccountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TermDepositAccount account = (TermDepositAccount)e.NewValue;
        }

        public ICommand UpdateAccountCommand
        {
            get { return (ICommand)GetValue(UpdateAccountCommandProperty); }
            set { SetValue(UpdateAccountCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpdateAccountCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdateAccountCommandProperty =
            DependencyProperty.Register("UpdateAccountCommand", typeof(ICommand), typeof(DepositAccountInfoTemplate), new PropertyMetadata(null));

    

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddBeneficiary_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.RequestedTheme = ThemeService.Theme;
            dialog.Title = "Add Beneficiary";
            dialog.Content = new AddEditBeneficiaryView(dialog);
            await dialog.ShowAsync();
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
            else if(SelectedAccount.RepaymentAccountNumber == RepaymentAccountNumberText.Text)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountUpdated += OnAccountUpdated;
        }

        private void OnAccountUpdated(bool isUpdated, AccountBObj updatedAccount)
        {
            if(isUpdated && updatedAccount is TermDepositAccount)
            {
                SelectedAccount = updatedAccount as TermDepositAccount;
            }
            else
            {
                Reset();
            }
        }

        private void Reset()
        {
            RepaymentAccountNumberText.Text = SelectedAccount.RepaymentAccountNumber;
            UpdateErrorText.Text = "";
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountUpdated -= OnAccountUpdated;
        }
    }
}
