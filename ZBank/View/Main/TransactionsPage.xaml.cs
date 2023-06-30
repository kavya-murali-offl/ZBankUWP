using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Config;
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
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewNotifier.Instance.OnRightPaneContentUpdated(null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
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
            dialog.RequestedTheme = ThemeSelector.Theme;
            dialog.Content = new NewPaymentView(dialog);
            PaymentDialog = dialog;
            await dialog.ShowAsync();
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateSelectedAccount(AccountsList.SelectedItem as AccountBObj);
            AccountNumberDropDownButton.Flyout.Hide();
        }

        private void FromAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.FilterValues["FromAccount"] = FromAccountsList.SelectedItem as AccountBObj;
            FromAccountButton.Flyout.Hide();
        }

        private void ToAccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.FilterValues["ToAccount"] = ToAccountsList.SelectedItem as AccountBObj;
            ToAccountButton.Flyout.Hide();  
        }

        private void TransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TransactionTypeButton.Flyout.Hide();    
        }

        private void ToDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

            ViewModel.FilterValues["ToDate"] = ToDatePicker.Date.ToString();
        }

        private void FromDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            ViewModel.FilterValues["FromDate"] = FromDatePicker.Date.ToString();
        }

        private void TransactionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionListView.SelectedValue != null)
            {
                TransactionBObj transactionBObj = (TransactionBObj)TransactionListView.SelectedItem;
                ViewModel.UpdateView(transactionBObj);
            }
        }
    }

}
