using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewAcountTemplates
{
    public sealed partial class NewDepositAccountFormTemplate : UserControl
    {
        public AddOrEditAccountViewModel ViewModel { get; set; }  
        
        public NewDepositAccountFormTemplate()
        {
            this.InitializeComponent();
            LoadAllAccounts();
        }

        public ObservableCollection<Account> AllAccounts { get; set; }

        public void LoadAllAccounts()
        {

        }

        public TermDepositAccount DepositAccount
        {
            get { return (TermDepositAccount)GetValue(DepositAccountProperty); }
            set { SetValue(DepositAccountProperty, value); }
        }

        public static readonly DependencyProperty DepositAccountProperty =
            DependencyProperty.Register("DepositAccount", typeof(TermDepositAccount), typeof(NewDepositAccountFormTemplate), new PropertyMetadata(null));



        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
        }

        private void TenureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView view)
            {
                ListView item = view;
                if (item.SelectedIndex >= 0)
                {
                    DepositAccount.TenureInMonths = (int)((item.SelectedItem as DropDownItem).Value);
                    TenureText.Text = (item.SelectedItem as DropDownItem).Text;
                }
            }

            TenureDropDownButton.Flyout.Hide();
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = null;
            if (sender is ListView view)
            {
                item = view;
            }
            if (item.SelectedIndex >= 0)
            {
                Account selectedAccount = (item.SelectedItem as Account);
               
            }
            AccountsDropdownButton.Flyout.Hide();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            AllAccounts = new ObservableCollection<Account>(args.AccountsList);
        }
    }

}
