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
using ZBank.View.Modals;
using ZBank.ViewModel;

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
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    ViewModel.UpdateUserAccount(item.SelectedIndex);
                }
                AccountsDropdownButton.Flyout.Hide();
            }
        }

        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    ViewModel.UpdateBeneficiary(item.SelectedIndex);
                }
                BeneficiaryButton.Flyout.Hide();
            }
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = ViewModel.UpdateAndGetAmount(sender.Text);
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
        }


        private ContentDialog PaymentDialog { get ; set; }  
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Steps.ElementAt(0).PrimaryCommand.Execute(null);
            //ContentDialog dialog = new ContentDialog();
            //dialog.Content = new NewPaymentView();
            //PaymentDialog = dialog;
            //await dialog.ShowAsync();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnloaded();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Steps.ElementAt(0).SecondaryCommand.Execute(null);
        }
    }
    }
