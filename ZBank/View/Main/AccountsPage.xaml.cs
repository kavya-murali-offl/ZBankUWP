﻿using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entity.BusinessObjects;
using ZBank.View.UserControls;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountsPage : Page, IView
    {

        public AccountPageViewModel ViewModel { get; set; }

        public AccountsPage()
        {
            this.InitializeComponent();
            ViewModel = new AccountPageViewModel(this);
            DataContext = ViewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
        }

        private void ViewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Account account = ((FrameworkElement)sender).DataContext as Account;

            AccountInfoPageParams parameters = new AccountInfoPageParams()
            {
                SelectedAccount = account,
            };

            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = typeof(AccountInfoFrame),
                Params = parameters
            };

            ViewNotifier.Instance.OnFrameContentChanged(args);
        }
    }
}
