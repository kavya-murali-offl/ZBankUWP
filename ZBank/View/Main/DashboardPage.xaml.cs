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
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPage : Page, IView
    {
        public int OnViewCardIndex { get; set; } = 0;   

        public DashboardViewModel ViewModel { get; set; }

        public string EnteredAmount { get; set; }  
        
        public string EnteredDescription { get; set; }   

        public Account SelectedAccount { get; set; }   

        public Beneficiary SelectedBeneficiary { get; set; }
        
        public ModeOfPayment SelectedPaymentMode { get; set; }   

        public DashboardPage()
        {
            this.InitializeComponent();
            ViewModel = new DashboardViewModel(this);
            //OnViewCardContent.DataContext = this;
            //OnViewCardContent.Content = (Resources["OnViewCardTemplate"] as DataTemplate).LoadContent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPreviousCard();
            //ViewModel.UpdateOnViewCard(DirectionButton.NEXT);
        }

      

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OnNextCard();
            //ViewModel.UpdateOnViewCard(DirectionButton.NEXT);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
           
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnLoaded();
        }

        private void QuickTransferAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void BeneficiaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = null;
            if (sender is ListView view)
            {
                 item = view;
            }
            if(item.SelectedIndex >= 0)
            {
                var beneficiaries = ViewModel.DashboardModel.AllBeneficiaries;
                SelectedBeneficiary = beneficiaries[item.SelectedIndex];
                BeneficiaryText.Text = beneficiaries[item.SelectedIndex]?.ToString();
            }
        }

        private void BeneficiaryButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AccountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView item = null;
            if (sender is ListView view)
            {
                item = view;
            }
            if(item.SelectedIndex >= 0)
            {
                var accountsList = ViewModel.DashboardModel.AllAccounts;
                SelectedAccount = accountsList[item.SelectedIndex];
                AccountsText.Text = accountsList[item.SelectedIndex]?.ToString();
            }
        }

        private void AmountTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            string newText = sender.Text;
            newText = new string(newText.Where(c => char.IsDigit(c) || c == '.').ToArray());
            sender.Text = newText;
            sender.SelectionStart = newText.Length;
        }


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedAccount = null;
            SelectedBeneficiary = null;
            SelectedPaymentMode = ModeOfPayment.NONE;
            AmountTextBox.Text = string.Empty;
            BeneficiaryList.SelectedIndex = -1;
            AccountsList.SelectedIndex = -1;
            BeneficiaryText.Text = "Select Beneficiary";
            AccountsText.Text = "Select Account";
            EnteredDescription = "";
        }

        private void DescriptionTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {

        }
    }
}

public enum DirectionButton
{
    NEXT,
    PREVIOUS
}
