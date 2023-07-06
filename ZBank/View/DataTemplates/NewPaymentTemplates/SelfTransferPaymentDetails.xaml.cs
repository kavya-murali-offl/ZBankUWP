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
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewPaymentTemplates
{
    public sealed partial class SelfTransferPaymentDetails : UserControl, IView
    {
        private TransferAmountViewModel ViewModel { get; set; }
        private bool IsConfirmed { get ; set; } 
        public SelfTransferPaymentDetails()
        {
            this.InitializeComponent();
            ViewModel = new TransferAmountViewModel(this);
            Reset();
        }

        private void Reset()
        {
            AccountsText.Text = "Select From Account";
            BeneficiaryText.Text = "Select To Account";
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsConfirmed)
            {
                ViewModel.CurrentTransaction.SenderAccountNumber = (AccountsList.SelectedValue as AccountBObj)?.AccountNumber;
                ViewModel.FieldErrors["Account"] = string.Empty;
                ViewModel.OtherAccounts = new ObservableCollection<AccountBObj>();
                foreach (var account in ViewModel.UserAccounts)
                {
                    if ((AccountsList?.SelectedValue as AccountBObj)?.AccountNumber != account.AccountNumber)
                    {
                        ViewModel.OtherAccounts.Add(account);
                    }
                }
                BeneficiaryText.Text = "Select To Account";
                ViewModel.CurrentTransaction.RecipientAccountNumber = null;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
                AccountsDropdownButton.Flyout.Hide();
            }
        }

        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsConfirmed)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = (BeneficiaryList.SelectedValue as AccountBObj)?.AccountNumber;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
                BeneficiaryButton.Flyout.Hide();
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(null);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;
        }

        private void CancelPaymentRequested(bool obj)
        {
            Reset();
            ViewModel.Reset();
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

        private void AccountsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!IsConfirmed)
            {
                ViewModel.CurrentTransaction.SenderAccountNumber = (AccountsList.SelectedValue as AccountBObj)?.AccountNumber;
                ViewModel.FieldErrors["Account"] = string.Empty;
                ViewModel.OtherAccounts = new ObservableCollection<AccountBObj>();
                foreach (var account in ViewModel.UserAccounts)
                {
                    if ((AccountsList?.SelectedValue as AccountBObj)?.AccountNumber != account.AccountNumber)
                    {
                        ViewModel.OtherAccounts.Add(account);
                    }
                }
                BeneficiaryText.Text = "Select To Acount";
                ViewModel.CurrentTransaction.RecipientAccountNumber = null;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
                AccountsDropdownButton.Flyout.Hide();
            }
        }

        private void BeneficiaryList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!IsConfirmed)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = (BeneficiaryList.SelectedValue as AccountBObj)?.AccountNumber;
                ViewModel.FieldErrors["Beneficiary"] = string.Empty;
                BeneficiaryButton.Flyout.Hide();
            }
        }
    }
}
