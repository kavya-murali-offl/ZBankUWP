using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
using ZBank.Entities.EnumerationType;
using ZBank.View.DataTemplates.NewAcountTemplates;
using ZBank.ViewModel;
using ZBank.ViewModel.VMObjects;
using ZBank.Services;
using ZBank.Utilities.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Modals
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddOrEditAccountPage : Page, IView
    {
        private AddOrEditAccountViewModel ViewModel { get; set; }


        public AddOrEditAccountPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
            ViewModel = new AddOrEditAccountViewModel(this);
        }


        private void AccountTypeButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons radios)
            {
                if(radios.SelectedItem != null) { 
                    AccountType accountType = (AccountType)radios.SelectedItem;
                    ViewModel.SelectedAccountType = accountType;
                    SetFormTemplate(accountType);
                }
            }
        }

        private void SetFormTemplate(AccountType accountType)
        {
            ViewModel.SwitchTemplate(accountType);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadContent();    
            AccountForm.DataContext = ViewModel;
            SetFormTemplate(AccountType.CURRENT);
        }


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UnloadContent();
        }

     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AccountTypeButton.SelectedIndex = 0;
            SetFormTemplate(AccountType.CURRENT);
        }


        private void BranchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    var branch = (item.SelectedItem as Branch);
                    ViewModel.UpdateBranch(branch);
                    BranchText.Text = branch.ToString();
                }
                BranchDropdown.Flyout.Hide();
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidateAndSubmit();
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UploadFiles();
        }
    }
}
