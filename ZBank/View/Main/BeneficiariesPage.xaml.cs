using System;
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
using ZBank.Config;
using ZBank.Entities;
using ZBank.Services;
using ZBank.View.UserControls;
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BeneficiariesPage : Page, IView
    {
        private BeneficiariesViewModel ViewModel { get; set; }  

        public BeneficiariesPage()
        {
            this.InitializeComponent();
            ViewModel = new BeneficiariesViewModel(this);   
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnloaded();
        }

        private void OtherBankTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var input = sender.Text;
            ViewModel.UpdateList(BeneficiaryType.OTHER_BANK, input);
        }

        private void WithinBankTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var input = sender.Text;
            ViewModel.UpdateList(BeneficiaryType.WITHIN_BANK, input);
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            BeneficiaryBObj selectedBeneficiary = ((FrameworkElement)sender).DataContext as BeneficiaryBObj;
            await ViewModel.OpenDialog(selectedBeneficiary);
        }

        private void NotAFavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            BeneficiaryBObj selectedBeneficiary = ((FrameworkElement)sender).DataContext as BeneficiaryBObj;
            ViewModel.SwitchFavourite(selectedBeneficiary);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BeneficiaryBObj selectedBeneficiary = ((FrameworkElement)sender).DataContext as BeneficiaryBObj;
            ViewModel.DeleteBeneficiary(selectedBeneficiary);
        }
    }
}
