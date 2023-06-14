using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Modals
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddOrEditAccountPage : Page, IView
    {
        private AddOrEditAccountViewModel ViewModel { get; set; }
        private IForm FormTemplate { get; set; }    

        public AddOrEditAccountPage()
        {
            this.InitializeComponent();
            ViewModel = new AddOrEditAccountViewModel(this);
            SetFormTemplate(AccountType.CURRENT);
           
        }


        private void AccountTypeButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is RadioButtons radios)
            {
                if(radios.SelectedItem != null) { 
                    AccountType accountType = (AccountType)radios.SelectedItem;
                    SetFormTemplate(accountType);
                }
            }
        }

        private IList<AccountType> AccountTypes { get; set; } = new List<AccountType>() { AccountType.CURRENT, AccountType.SAVINGS, AccountType.TERM_DEPOSIT };

        private void SetFormTemplate(AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.CURRENT:
                    NewCurrentAccountFormTemplate newCurrentAccountFormTemplate = new NewCurrentAccountFormTemplate();
                    newCurrentAccountFormTemplate.SubmitCommand = ViewModel.SubmitCommand;
                    FormTemplate = newCurrentAccountFormTemplate;
                    break;
                case AccountType.SAVINGS:
                    NewSavingsAccountFormTemplate newSavingsAccountFormTemplate = new NewSavingsAccountFormTemplate();
                    newSavingsAccountFormTemplate.SubmitCommand = ViewModel.SubmitCommand;
                    FormTemplate = newSavingsAccountFormTemplate;
                    break;
                case AccountType.TERM_DEPOSIT:
                    NewDepositAccountFormTemplate newDepositAccountFormTemplate = new NewDepositAccountFormTemplate();
                    newDepositAccountFormTemplate.SubmitCommand = ViewModel.SubmitCommand;
                    FormTemplate = newDepositAccountFormTemplate;
                    break;
            }
            AccountForm.Content = FormTemplate;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountForm.DataContext = ViewModel;
            NewCurrentAccountFormTemplate newCurrentAccountFormTemplate = new NewCurrentAccountFormTemplate();
            AccountForm.Content = newCurrentAccountFormTemplate;
            ViewNotifier.Instance.ThemeChanged += ChangeTheme;
            ViewNotifier.Instance.OnSuccess += InsertedAccount;
            ViewModel.LoadContent();
        }

        private void InsertedAccount(bool obj)
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
            ViewNotifier.Instance.OnSuccess -= InsertedAccount;
            ViewModel.UnloadContent();
        }

        private async void ChangeTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = RequestedTheme = theme;
            });
        }

        private void BranchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            FormTemplate.ValidateAndSubmit();
        }
    }
}
