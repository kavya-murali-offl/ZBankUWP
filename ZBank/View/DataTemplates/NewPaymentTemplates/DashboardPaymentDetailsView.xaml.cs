using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
using ZBank.View.Modals;
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewPaymentTemplates
{
    public sealed partial class DashboardPaymentDetailsView : UserControl, IView
    {
        private TransferAmountViewModel ViewModel { get; set; }

        public DashboardPaymentDetailsView()
        {
            this.InitializeComponent();
            ViewModel = new TransferAmountViewModel(this, TransactionType.TRANSFER);
            Reset();
        }


        private void Reset()
        {
            AccountsSuggestionBox.Text  = string.Empty; 
            BeneficiariesSuggestionBox.Text = string.Empty;
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            sender.Text = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.SelectionStart = newText.Length;
            ViewModel.CurrentTransaction.Amount = decimal.TryParse(newText, out decimal amount) ? amount : 0m;
            ViewModel.FieldErrors["Amount"] = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;

        }

        private void CancelPaymentRequested(bool obj)
        {
            Reset();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnloaded();
            ViewNotifier.Instance.CancelPaymentRequested -= CancelPaymentRequested;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Steps.ElementAt(0).SecondaryCommand.Execute(null);
            Reset();
        }

        private void DescriptionTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(true);
            }
        }

        private void AmountTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(true);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var account = args.SelectedItem as AccountBObj;
            sender.Text = account.ToString();
            ViewModel.CurrentTransaction.SenderAccountNumber = account?.AccountNumber;
            ViewModel.FieldErrors["Account"] = string.Empty;
        }

        private void AccountsSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.SenderAccountNumber = string.Empty;
                }
            }
        }

        private void BeneficiariesSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.SenderAccountNumber = string.Empty;
                }
            }
        }

        private void BeneficiariesSuggestionBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem != null && args.SelectedItem is BeneficiaryBObj beneficiary)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = beneficiary.AccountNumber;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
            }
        }
    }
}
