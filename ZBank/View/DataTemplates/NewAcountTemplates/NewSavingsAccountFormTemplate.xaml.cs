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
    public sealed partial class NewSavingsAccountFormTemplate : UserControl
    {
        public NewSavingsAccountFormTemplate()
        {
            this.InitializeComponent();
            Reset();
        }
        public ObservableDictionary<string, object> FieldValues
        {
            get { return (ObservableDictionary<string, object>)GetValue(FieldValuesProperty); }
            set { SetValue(FieldValuesProperty, value); }
        }
        public void Reset()
        {
            FieldValues["Amount"] = string.Empty;
            FieldErrors["Amount"] = string.Empty;
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

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
            FieldValues["Amount"] = newText;
            if(newText?.Length > 0)
            {
                FieldErrors["Amount"] = string.Empty;
            }
        }

        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(NewSavingsAccountFormTemplate), new PropertyMetadata(null));

        
    }
}
