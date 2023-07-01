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
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           AccountsDropdownButton.Flyout.Hide();
        }

        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BeneficiaryButton.Flyout.Hide();
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            sender.Text = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.SelectionStart = newText.Length;
        }

        private void DescriptionTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {

        }
    }
}
