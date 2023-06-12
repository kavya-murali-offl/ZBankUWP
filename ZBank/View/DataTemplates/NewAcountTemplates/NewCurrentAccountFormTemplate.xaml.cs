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
using ZBank.Entities;
using ZBank.ViewModel.VMObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewAcountTemplates
{
    public sealed partial class NewCurrentAccountFormTemplate : UserControl, IForm
    {
        public NewCurrentAccountFormTemplate()
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

        public void ValidateAndSubmit() 
        {
            if (ValidateFields())
            {
                CurrentAccount currentAccount = new CurrentAccount()
                {
                    AccountName = "Kavya",
                    AccountStatus = Entities.EnumerationType.AccountStatus.INACTIVE,
                    Balance = decimal.Parse(FieldValues["Deposit Amount"].ToString()),
                    AccountType = Entities.EnumerationType.AccountType.CURRENT,
                    UserID = "1111",
                    CreatedOn = DateTime.Now,
                };
                SubmitCommand.Execute(currentAccount);
            }
        }

        public void Reset()
        {
            FieldValues["Deposit Amount"] = string.Empty;
            FieldErrors["Deposit Amount"] = string.Empty;
        }

        private bool ValidateFields()
        {
            foreach (var key in FieldValues.Keys)
            {
                ValidateField(key);
            }

                var inText = FieldValues["Deposit Amount"].ToString();
                if (decimal.TryParse(inText, out decimal amountInDecimal))
                {
                    FieldErrors["Deposit Amount"] = string.Empty;
                }
                else
                {
                    FieldErrors["Deposit Amount"] = "Please enter a valid deposit Amount";
                }

            if (FieldErrors.Values.Any((val) => val.Length > 0))
                return false;
            return true;
        }
    }
}
