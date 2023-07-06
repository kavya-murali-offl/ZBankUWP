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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.Modals
{
    public sealed partial class PayCreditCard : UserControl, IView
    {
        public PayCreditCard()
        {
            this.InitializeComponent();
            AccountsText.Text = "Select Account";
        }

        private PayCreditCardViewModel ViewModel { get; set; }      

        private ContentDialog ContentDialog { get; set; }

        private void OnCardSettled(bool IsSettled)
        {
            ContentDialog?.Hide();
        }

        public PayCreditCard(ContentDialog dialog, CreditCard card)
        {
            this.InitializeComponent();
            ViewModel = new PayCreditCardViewModel(this, dialog, card);
        }

        private void AmountToSettleBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            sender.Text = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.SelectionStart = newText.Length;
            ViewModel.Amount = decimal.TryParse(newText, out decimal amount) ? amount : 0m;
            ViewModel.FieldErrors["Amount"] = string.Empty;
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedAccount = (AccountsList.SelectedValue as AccountBObj);
            ViewModel.AvailableBalance = ViewModel.SelectedAccount.Balance;
            ViewModel.FieldErrors["Account"] = string.Empty;
            AccountsDropdownButton.Flyout.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedAccount = null;
            ViewModel.AvailableBalance = 0;
            AccountsText.Text = "Select Account";
            ViewModel.Reset();
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PayCard();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CloseDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CreditCardSettled += OnCardSettled;
            ViewModel.OnLoaded();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CreditCardSettled -= OnCardSettled;
            ViewModel.OnUnloaded();
        }
    }
}
