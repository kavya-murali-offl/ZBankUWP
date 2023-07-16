using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewPaymentTemplates
{
    public sealed partial class SelfTransferPaymentDetails : UserControl, IView
    {
        private TransferAmountViewModel ViewModel { get; set; }

        public SelfTransferPaymentDetails()
        {
            this.InitializeComponent();
            ViewModel = new TransferAmountViewModel(this, TransactionType.SELF_TRANSFER);
            Reset();
        }

        private void Reset()
        {
            FromAccount.Text = string.Empty;
            ToAccount.Text = string.Empty;  
        }

        private void ToAccount_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.SenderAccountNumber = string.Empty;

                }
            }
        }

        private void FromAccount_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem != null && args.SelectedItem is AccountBObj account)
            {
                ViewModel.CurrentTransaction.SenderAccountNumber = account.AccountNumber;
                ViewModel.FieldErrors["Account"] = string.Empty;
                ViewModel.OtherAccounts = new ObservableCollection<AccountBObj>();
                foreach (var otherAccount in ViewModel.UserAccounts)
                {
                    if (account?.AccountNumber != otherAccount.AccountNumber)
                    {
                        ViewModel.OtherAccounts.Add(otherAccount);
                    }
                }
                ViewModel.CurrentTransaction.RecipientAccountNumber = null;
                ToAccount.Text = string.Empty;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
            }
        }


        private void FromAccount_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.CurrentTransaction.SenderAccountNumber = string.Empty;
                    ViewModel.OtherAccounts = ViewModel.UserAccounts;
                    ViewModel.CurrentTransaction.RecipientAccountNumber = null;
                    Reset();
                }
            }
        }

        private void ToAccount_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null && args.SelectedItem is AccountBObj beneficiary)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = beneficiary.AccountNumber;
                ToAccount.Text = beneficiary.ToString();
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
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
            ViewModel.Reset(TransactionType.SELF_TRANSFER);
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

        private void AmountTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                //IsConfirmed = true;
                ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(true);
            }
        }

        private void FromAccount_LostFocus(object sender, RoutedEventArgs e)
        {
            FromAccount.IsSuggestionListOpen = false;
        }

        private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ToAccount.IsSuggestionListOpen = false;
        }
    }
}
