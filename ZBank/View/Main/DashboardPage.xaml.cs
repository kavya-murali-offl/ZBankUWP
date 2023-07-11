using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View.UserControls;
using ZBank.ViewModel;
using ZBank.View.Main;
using ZBank.View.Modals;
using System.Threading.Tasks;
using ZBank.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, IView
    {

        public DashboardViewModel ViewModel { get; set; }

        public DashboardPage()
        {
            this.InitializeComponent();
            ViewModel = new DashboardViewModel(this); }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnLoaded();
        }

        private void Transactions_ViewMoreButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnViewMoreTransactions();
        }

        private void ManageCardButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ManageCard();
        }

        private async void NewCardButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.OpenAddCreditCardDialog();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FadeStoryboard.Begin();
            await Task.Delay(500);
            FadeStoryboard.Stop();
        }
    }
}

public enum DirectionButton
{
    NEXT,
    PREVIOUS
}
