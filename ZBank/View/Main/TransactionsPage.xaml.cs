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
using ZBank.Services;
using ZBank.View.Modals;
using ZBank.View.UserControls;
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
            ToDatePicker.Date = DateTime.Now;
            FromDatePicker.Date = DateTime.Now.AddMonths(-1);
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
                ViewModel.UpdateRows(rows);
            }
            RowsPerPageButton.Flyout.Hide();

        }

        private async void NewPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.OpenNewPaymentDialog();
        }


        private void ToDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            ViewModel.UpdateToDate(sender.Date.GetValueOrDefault().Date);
        }

        private void FromDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            ViewModel.UpdateFromDate(sender.Date.GetValueOrDefault().Date);
        }

        private void TransactionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionListView.SelectedValue != null)
            {
                TransactionBObj transactionBObj = (TransactionBObj)TransactionListView.SelectedItem;
                ViewModel.UpdateView(transactionBObj);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var account = args.SelectedItem as AccountBObj;
            sender.Text = account.ToString();
            ViewModel.UpdateSelectedAccount(account);
        }

        private void AccountsSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text))
                {
                    ViewModel.UpdateSelectedAccount(null);
                }
            }
        }

        private void AccountsSuggestionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AccountsSuggestionBox.IsSuggestionListOpen = false;
        }
    }
}


