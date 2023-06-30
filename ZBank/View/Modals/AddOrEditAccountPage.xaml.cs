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
            switch (accountType)
            {
                case AccountType.CURRENT:
                    NewCurrentAccountFormTemplate newCurrentAccountFormTemplate = new NewCurrentAccountFormTemplate()
                    {
                        SubmitCommand = ViewModel.SubmitCommand,
                        FieldErrors = ViewModel.FieldErrors,
                        FieldValues = ViewModel.FieldValues,
                    };
                    newCurrentAccountFormTemplate.Reset();
                    AccountForm.Content = newCurrentAccountFormTemplate;
                    break;
                case AccountType.SAVINGS:
                    NewSavingsAccountFormTemplate newSavingsAccountFormTemplate = new NewSavingsAccountFormTemplate()
                    {
                        SubmitCommand = ViewModel.SubmitCommand,
                        FieldErrors = ViewModel.FieldErrors,
                        FieldValues = ViewModel.FieldValues,
                    };
                    newSavingsAccountFormTemplate.Reset();
                    AccountForm.Content = newSavingsAccountFormTemplate;
                    break;
                case AccountType.TERM_DEPOSIT:
                    NewDepositAccountFormTemplate newDepositAccountFormTemplate = new NewDepositAccountFormTemplate()
                    {
                        SubmitCommand = ViewModel.SubmitCommand,
                        FieldErrors = ViewModel.FieldErrors,
                        FieldValues = ViewModel.FieldValues,
                    };
                    newDepositAccountFormTemplate.Reset();
                    AccountForm.Content = newDepositAccountFormTemplate;
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountForm.DataContext = ViewModel;
            NewCurrentAccountFormTemplate newCurrentAccountFormTemplate = new NewCurrentAccountFormTemplate();
            AccountForm.Content = newCurrentAccountFormTemplate;
            ViewNotifier.Instance.ThemeChanged += ChangeTheme;
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
            ViewNotifier.Instance.AccountInserted += OnAccountInsertionSuccessful;
            ViewModel.LoadContent();
        }

        private void OnAccountInsertionSuccessful(bool obj)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
            ViewModel.UnloadContent();
            ViewModel.CloseView();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
            ViewModel.UnloadContent();
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }

        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
            ViewModel.UnloadContent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new AddOrEditAccountViewModel(this);
            AccountTypeButton.SelectedIndex = 0;
            SetFormTemplate(AccountType.CURRENT);
            ViewModel.Initialize(e.Parameter as ViewLifetimeControl);
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
            PickFilesOutputTextBlock.Text = "";

            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add("*");

            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("Uploaded Files");
                foreach (StorageFile file in files)
                {
                    output.Append(file.Name + "\n");
                }
                PickFilesOutputTextBlock.Text = output.ToString();
                ViewModel.FieldValues["KYC"] = files;
                ViewModel.FieldErrors["KYC"] = string.Empty;
            }
            else
            {
                PickFilesOutputTextBlock.Text = "";
            }
        }
    }
}
