using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page, IView
    {
        public DashboardViewModel DashboardViewModel { get; set; }


        public Home()
        {
            this.InitializeComponent();
            DashboardViewModel = new DashboardViewModel(this);
        }

       
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //if (CardList.SelectedIndex > 0)
            //{
            //    CardList.SelectedIndex--;
            //}
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            //if (CardList.SelectedIndex < CardList.Items.Count - 1)
            //{
            //    CardList.SelectedIndex++;
            //}
        }
    }
}
