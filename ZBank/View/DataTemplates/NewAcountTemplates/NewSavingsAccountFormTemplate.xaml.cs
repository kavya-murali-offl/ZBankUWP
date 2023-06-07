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
using ZBank.ViewModel.VMObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewAcountTemplates
{
    public sealed partial class NewSavingsAccountFormTemplate : UserControl
    {
        public NewSavingsAccountFormTemplate()
        {
            this.InitializeComponent();
            FieldValues["Deposit Amount"] = string.Empty;
            FieldErrors["Deposit Amount"] = string.Empty;
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

            if (fieldName == "Deposit Amount")
            {
                var inText = FieldValues["Deposit Amount"].ToString();
                if (decimal.TryParse(inText, out decimal amountInDecimal))
                {
                    FieldValues["Deposit Amount"] = inText;
                    FieldErrors["Deposit Amount"] = string.Empty;
                }
                else
                {
                    FieldErrors["Deposit Amount"] = "Please enter a valid deposit Amount";
                }
            }
        }

        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(NewDepositAccountFormTemplate), new PropertyMetadata(null));

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
            FieldValues["Deposit Amount"] = newText;
            ValidateField("Deposit Amount");
        }

        //private void SubmitButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ValidateFields())
        //    {
        //        TermDepositAccount depositAccount = new TermDepositAccount()
        //        {
        //            AccountName = "Kavya",
        //            AccountStatus = Entities.EnumerationType.AccountStatus.INACTIVE,
        //            Balance = (decimal)FieldValues["Deposit Amount"],
        //            AccountType = Entities.EnumerationType.AccountType.TERM_DEPOSIT,
        //            TenureInMonths = int.Parse(FieldValues["Tenure"].ToString()),
        //            RepaymentAccountNumber = FieldValues["Repayment Account Number"].ToString(),
        //            UserID = "1111",
        //            CreatedOn = DateTime.Now,
        //            DepositStartDate = null,
        //            DepositType = Entity.EnumerationTypes.DepositType.OnMaturity
        //        };
        //        depositAccount.SetDefault();
        //        SubmitCommand.Execute(depositAccount);
        //    }
        //}

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
    }
}
