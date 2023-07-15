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
using ZBank.Entities.BusinessObjects;
using ZBank.Entities;
using ZBank.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using ZBankManagement.Entities.BusinessObjects;
using ZBank.AppEvents;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewPaymentTemplates
{
    public sealed partial class PaymentDetails : UserControl
    {
        private TransferAmountViewModel ViewModel { get; set; } 

        public PaymentDetails()
        {
            this.InitializeComponent();
            ViewModel = DataContext as TransferAmountViewModel;
            DataContextChanged += (s, e) => ViewModel = DataContext as TransferAmountViewModel;
            Reset();
        }

        private void Accounts_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null && args.SelectedItem is AccountBObj selectedAccount)
            {
                ViewModel.CurrentTransaction.SenderAccountNumber = selectedAccount?.AccountNumber;
                ViewModel.AvailableBalance = selectedAccount?.Balance ?? 0;
                ViewModel.FieldErrors["Account"] = string.Empty;
            }
        }

        private void Accounts_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.SenderAccountNumber = string.Empty;
                    ViewModel.AvailableBalance = 0m;
                }
            }
        }


        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            sender.Text = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.SelectionStart = newText.Length;
            ViewModel.CurrentTransaction.Amount = decimal.TryParse(newText, out decimal amount) ? amount : 0m;
            ViewModel.FieldErrors["Amount"] = string.Empty;
        }

        private void DescriptionTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.PaymentResetRequested += Reset;
        }

        private void Reset()
        {
            AccountsBox.Text = string.Empty;
            BeneficiariesBox.Text = string.Empty;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.PaymentResetRequested -= Reset;
        }

        private void BeneficiariesBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.RecipientAccountNumber = string.Empty;
                }
            }
        }

        private void BeneficiariesBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null && args.SelectedItem is BeneficiaryBObj beneficiary)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = beneficiary.AccountNumber;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
            }
        }
    }
}
