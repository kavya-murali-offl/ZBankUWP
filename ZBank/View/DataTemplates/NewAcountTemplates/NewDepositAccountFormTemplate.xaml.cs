using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Utilities.Validation;
using ZBank.ViewModel;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewAcountTemplates
{
    public sealed partial class NewDepositAccountFormTemplate : UserControl, IForm
    {
        public NewDepositAccountFormTemplate()
        {
            this.InitializeComponent();
            Reset();
        }

        private ObservableDictionary<string, string> FieldErrors = new ObservableDictionary<string, string>();
        private ObservableDictionary<string, object> FieldValues = new ObservableDictionary<string, object>();

        public void ValidateField(string fieldName)
        {
            if (!FieldValues.TryGetValue(fieldName, out object val) || string.IsNullOrEmpty(FieldValues[fieldName]?.ToString()))
            {
                FieldErrors[fieldName] = $"{fieldName} is required.";
            }
            else
            {
                FieldErrors[fieldName] = string.Empty;
            }

            if(fieldName == "Deposit Amount")
            {
                var inText = FieldValues["Deposit Amount"].ToString();
                if (decimal.TryParse(inText, out decimal amountInDecimal))
                {
                    FieldErrors["Deposit Amount"] = string.Empty;
                }
                else
                {
                    FieldErrors["Deposit Amount"] = "Please enter a valid deposit Amount";
                }
            }
        }

        public void Reset() {
            FieldErrors.Add("Deposit Amount", string.Empty);
            FieldErrors.Add("Repayment Account Number", string.Empty);
            FieldErrors.Add("From Account Number", string.Empty);
            FieldErrors.Add("Tenure", string.Empty);

            FieldValues["Deposit Amount"] = string.Empty;
            FieldValues["From Account Number"] = string.Empty;
            FieldValues["Repayment Account Number"] = string.Empty;
            FieldValues["Tenure"] = string.Empty;
            FieldValues["Interest Rate"] = "0.0%";
        }

        public void ValidateAndSubmit()
        {
            if (ValidateFields())
            {
                TermDepositAccount depositAccount = new TermDepositAccount()
                {
                    AccountName = "Kavya",
                    AccountStatus = Entities.EnumerationType.AccountStatus.INACTIVE,
                    Balance = decimal.Parse(FieldValues["Deposit Amount"].ToString()),
                    AccountType = Entities.EnumerationType.AccountType.TERM_DEPOSIT,
                    TenureInMonths = int.Parse(FieldValues["Tenure"].ToString()),
                    RepaymentAccountNumber = FieldValues["Repayment Account Number"].ToString(),
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                    DepositStartDate = null,
                    DepositType = Entity.EnumerationTypes.DepositType.OnMaturity
                };
                depositAccount.SetDefault();
                SubmitCommand.Execute(depositAccount);
            }
        }

        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(NewSavingsAccountFormTemplate), new PropertyMetadata(null));

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
            FieldValues["Deposit Amount"] = newText;
            ValidateField("Deposit Amount");
        }

        private void TenureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView view)
            {
                ListView item = view;
                if (item.SelectedIndex >= 0)
                {
                    FieldValues["Tenure"] = (item.SelectedItem as DropDownItem).Value;
                    TenureText.Text = (item.SelectedItem as DropDownItem).Text;
                    UpdateInterestRate();
                }
            }
            ValidateField("Tenure");
            TenureDropDownButton.Flyout.Hide();
        }

        private void UpdateInterestRate()
        {
            if (int.TryParse(FieldValues["Tenure"].ToString(), out int tenure))
            {
                decimal interestRate = TermDepositAccount.GetFDInterestRate(tenure);
                FieldValues["Interest Rate"] = interestRate.ToString() + "%";
            }
        }

        private void FromAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = null;
            if (sender is ListView view)
            {
                item = view;
            }
            if (item.SelectedIndex >= 0)
            {
                Account selectedAccount = (item.SelectedItem as Account);
                FieldValues["From Account Number"] = selectedAccount.AccountNumber;
                FromAccountText.Text = selectedAccount.ToString();
                FromAccountDropdownButton.Flyout.Hide();
            }
            ValidateField("From Account Number");

        }

        private void ToAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = null;
            if (sender is ListView view)
            {
                item = view;
            }
            if (item.SelectedIndex >= 0)
            {
                Account selectedAccount = (item.SelectedItem as Account);
                FieldValues["Repayment Account Number"] = selectedAccount.AccountNumber;
                ToAccountText.Text = selectedAccount.ToString();
                ToAccountDropdownButton.Flyout.Hide();
            }
            ValidateField("Repayment Account Number");

        }


        private bool ValidateFields() 
        {
            foreach (var key in FieldValues.Keys)
            {
                ValidateField(key);
            }

            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;
            return true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        
    }

}
