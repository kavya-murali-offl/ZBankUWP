using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
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
    public sealed partial class AccountsPage : Page, IView
    {

        public AccountPageViewModel ViewModel { get; set; }

        public AccountsPage()
        {
            this.InitializeComponent();
            ViewModel = new AccountPageViewModel(this);
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
            AccountBObj account = ((FrameworkElement)sender).DataContext as AccountBObj;
            ViewModel.NavigateToInfoPage(account);
           
        }

        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            await WindowService.ShowAsync<AddOrEditAccountPage>(true);
        }
    }
}
