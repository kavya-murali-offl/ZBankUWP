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
            ViewModel = new TransferAmountViewModel(this);
            Reset();
        }


        private void Reset()
        {
            AccountsText.Text = "SelectAccount".GetLocalized();
            BeneficiaryText.Text = "SelectBeneficiary".GetLocalized();
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.IsConfirmed)
            {
                ViewModel.CurrentTransaction.SenderAccountNumber = (AccountsList.SelectedValue as AccountBObj)?.AccountNumber;
                ViewModel.FieldErrors["Account"] = string.Empty;
                AccountsDropdownButton.Flyout.Hide();
            }
            
        }

        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.IsConfirmed)
            {
                ViewModel.CurrentTransaction.RecipientAccountNumber = (BeneficiaryList.SelectedValue as BeneficiaryBObj)?.AccountNumber;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            
        }

        private void BeneficiaryList_ItemClick(object sender, ItemClickEventArgs e)
        {
 
        }

        private void DescriptionTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(null);
            }
        }

        private void AmountTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(null);
            }
        }
    }
}
