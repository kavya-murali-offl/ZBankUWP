﻿using System;
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
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
            ViewNotifier.Instance.CancelPaymentRequested += CancelPaymentRequested;
        }

        private void ArrowColumn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelPaymentRequested(bool isPaymentCompleted)
        {
            if(PaymentDialog != null)
            {
                PaymentDialog.Hide();
                PaymentDialog = null;   
            }
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
            dialog.Content = new NewPaymentView();
            PaymentDialog = dialog;
            await dialog.ShowAsync();
        }
    }

}
