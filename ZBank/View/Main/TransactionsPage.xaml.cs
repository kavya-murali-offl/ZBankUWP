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
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View.Modals;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionsPage : Page, IView
    {
        public TransactionViewModel ViewModel { get; set; }  
        public ContentDialog PaymentDialog { get; set; }  

        public TransactionsPage()
        {
            this.InitializeComponent();
            ViewModel = new TransactionViewModel(this);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
        }

        private void ArrowColumn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RowsPerPageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(int.TryParse(RowsPerPageList.SelectedItem.ToString(), out int rows))
            {
                ViewModel.RowsPerPage = rows;
            }
            RowsPerPageButton.Flyout.Hide();

        }

        private async void NewPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Content = new NewPaymentView(dialog);
            PaymentDialog = dialog;
            await dialog.ShowAsync();
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    var account = (item.SelectedItem as AccountBObj);
                    ViewModel.UpdateSelectedAccount(account);
                }
                AccountNumberDropDownButton.Flyout.Hide();
            }
        }

        private void FromAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FromAccountButton.Flyout.Hide();
        }

        private void ToAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToAccountButton.Flyout.Hide();  
        }

        private void TransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TransactionTypeButton.Flyout.Hide();    
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }

        private void ToDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }

        private void FromDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }
    }

}
