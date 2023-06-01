using Microsoft.UI.Xaml.Controls;
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
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Modals
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddOrEditAccountPage : Page, IView
    {

        public AddOrEditAccountViewModel ViewModel { get; set; }

        public AddOrEditAccountPage()
        {
            this.InitializeComponent();
            ViewModel = new AddOrEditAccountViewModel(this);
        }

       

        private void AccountTypeButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons radios)
            {
                string accountType = radios.SelectedItem as string;
                DataTemplate template = null;

                switch (accountType)
                {
                    case "Current":
                        template = Resources["CurrentAccountFormTemplate"] as DataTemplate;
                        break;
                    case "Savings":
                        template = Resources["SavingsAccountFormTemplate"] as DataTemplate;
                        break;
                    case "Deposit":
                        template = Resources["DepositAccountFormTemplate"] as DataTemplate;
                        break;
                }

                if (template != null)
                {
                    AccountForm.DataContext = ViewModel;
                    AccountForm.Content = template.LoadContent();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountForm.DataContext = ViewModel;
            DataTemplate template = Resources["CurrentAccountFormTemplate"] as DataTemplate;
            AccountForm.Content = template.LoadContent();
        }
    }

    
}
