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
    public sealed partial class NewDepositAccountFormTemplate : UserControl
    {
        public NewDepositAccountFormTemplate()
        {
            this.InitializeComponent();
            Reset();
        }

        public ObservableDictionary<string, object> FieldValues
        {
            get { return (ObservableDictionary<string, object>)GetValue(FieldValuesProperty); }
            set { SetValue(FieldValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldValuesProperty =
            DependencyProperty.Register("FieldValues", typeof(ObservableDictionary<string, object>), typeof(NewCurrentAccountFormTemplate), new PropertyMetadata(new ObservableDictionary<string, object>()));

        public ObservableDictionary<string, string> FieldErrors
        {
            get { return (ObservableDictionary<string, string>)GetValue(FieldErrorsProperty); }
            set { SetValue(FieldErrorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldErrors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldErrorsProperty =
            DependencyProperty.Register("FieldErrors", typeof(ObservableDictionary<string, string>), typeof(NewCurrentAccountFormTemplate), new PropertyMetadata(new ObservableDictionary<string, string>()));

       public void Reset()
        {
            FieldValues["Amount"] = string.Empty;
            FieldErrors["Amount"] = string.Empty;
            FieldValues["Repayment Account Number"] = null;
            FieldErrors["Repayment Account Number"] = string.Empty;
            FieldValues["From Account Number"] = null;
            FieldErrors["From Account Number"] = string.Empty;
            FieldValues["Tenure"] = string.Empty;
            FieldErrors["Tenure"] = string.Empty;
            FieldValues["Interest Rate"] = "0.0";
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
            FieldValues["Amount"] = newText;
            if (newText?.Length > 0)
            {
                FieldErrors["Amount"] = string.Empty;
            }
        }

        private void TenureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView view)
            {
                ListView item = view;
                if (item.SelectedIndex >= 0)
                {
                    FieldValues["Tenure"] = (item.SelectedItem as DropDownItem).Value;
                    FieldErrors["Tenure"] = string.Empty;
                    TenureText.Text = (item.SelectedItem as DropDownItem).Text;
                    UpdateInterestRate();
                }
            }
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

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void ToAccountsSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    FieldValues["From Account Number"] = string.Empty;
                }
            }
        }

        private void ToAccountsSuggestionBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null && args.SelectedItem is Account account)
            {
                FieldValues["From Account Number"] = account.ToString();
                FieldErrors["From Account Number"] = string.Empty;
            }
        }

        private void AccountsSuggestionBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem != null && args.SelectedItem is Account account)
            {
                FieldValues["Repayment Account Number"] = account.ToString();
                FieldErrors["Repayment Account Number"] = string.Empty;
            }
        }

        private void AccountsSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    FieldValues["Repayment Account Number"] = string.Empty;
                }
            }
        }

        private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AccountsSuggestionBox.IsSuggestionListOpen = false;
        }

        private void ToAccountsSuggestionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ToAccountsSuggestionBox.IsSuggestionListOpen = false;
        }
    }

}
