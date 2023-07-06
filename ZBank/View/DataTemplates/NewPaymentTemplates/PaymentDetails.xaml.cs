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

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Account selectedAccount = (AccountsList.SelectedValue as Account);
            ViewModel.CurrentTransaction.SenderAccountNumber = selectedAccount?.AccountNumber;
            ViewModel.AvailableBalance = selectedAccount?.Balance ?? 0;
            ViewModel.FieldErrors["Account"] = string.Empty;
            AccountsDropdownButton.Flyout.Hide();
        }

        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.CurrentTransaction.RecipientAccountNumber = (BeneficiaryList.SelectedValue as BeneficiaryBObj)?.AccountNumber;
            ViewModel.FieldErrors["Beneficiary"] = string.Empty;
            BeneficiaryButton.Flyout.Hide();
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
            AccountsText.Text = "Select Account";
            BeneficiaryText.Text = "Select Beneficiary";
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.PaymentResetRequested -= Reset;
        }
    }
}
