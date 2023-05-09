using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, IView
    {
        public DashboardViewModel ViewModel { get; set; } 

        public IDictionary<string, object> BalanceCard;

        public DashboardPage()
        {
            this.InitializeComponent();
            ViewModel = new DashboardViewModel(this);
            BalanceCard = new Dictionary<string, object>
            {
                { "Total Balance", 15000m },
                { "Total Savings", 5000m },
                { "Total Deposit", 10000m },
            };
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }


}
