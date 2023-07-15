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

       

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadContent();    
            AccountForm.DataContext = ViewModel;
            RadioButton1.IsChecked = true;
        }


        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.UnloadContent();
        }

     
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidateAndSubmit();
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UploadFiles();
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem == null)
            {
                ViewModel.UpdateBranch(null);
            }
            else
            {
                var branch = args.SelectedItem as Branch;
                ViewModel.UpdateBranch(branch);
                sender.Text = branch.ToString();
            }
        }

        private void BranchSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(sender.Text)){
                    ViewModel.UpdateBranch(null);
                }
            }
        }

        private void BranchSuggestionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            BranchSuggestionBox.IsSuggestionListOpen = false;
        }

        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateAccounType(0);
        }

        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateAccounType(1);
        }

        private void RadioButton3_Checked(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateAccounType(2);
        }
    }
}
