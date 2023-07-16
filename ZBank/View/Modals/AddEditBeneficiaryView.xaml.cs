using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ZBank.Entities;
using ZBank.Services;
using ZBank.ViewModel;
using ZBankManagement.Entities.BusinessObjects;
using ZBankManagement.Entity.EnumerationTypes;
using ZBankManagement.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class AddEditBeneficiaryView : UserControl, IView
    {
        private AddEditBeneficiaryViewModel ViewModel { get; set; }

        public AddEditBeneficiaryView(bool isModal=false)
        {
            this.InitializeComponent();
            ViewModel = new AddEditBeneficiaryViewModel(this);
            BeneficiaryButton.Content = ViewModel.BeneficiaryTypes.ElementAt(0).ToString().GetLocalized();
            IsModal = isModal;
            if (isModal) ContentGrid.MinWidth = 400;
        }

        public AddEditBeneficiaryView()
        {
            this.InitializeComponent();
            ViewModel = new AddEditBeneficiaryViewModel(this);
            BeneficiaryButton.Content = ViewModel.BeneficiaryTypes.ElementAt(0).ToString().GetLocalized();
        }

        public bool IsModal { get; set; }

        public AddEditBeneficiaryView(BeneficiaryBObj beneficiaryBObj, bool isModal)
        {
            this.InitializeComponent();
            ViewModel = new AddEditBeneficiaryViewModel(this, beneficiaryBObj);
            IsModal = isModal;
            if (isModal) ContentGrid.MinWidth = 400;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView item)
            {
                if (item.SelectedIndex >= 0)
                {
                    ViewModel.SetBeneficiaryType(item.SelectedIndex);
                    BeneficiaryButton.Content = item.SelectedValue.ToString().GetLocalized(); ;
                }
                BeneficiaryButton.Flyout.Hide();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged += OnThemeChanged;
            ViewModel.OnLoaded();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewModel.OnUnloaded();

        }

        private async void OnThemeChanged(ElementTheme obj)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                this.RequestedTheme = obj;
            });
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.OnCloseDialog();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Reset(ViewModel.EditableItem.BeneficiaryType);
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                SubmitForm();
            }
        }

        private void SubmitForm()
        {
            ViewModel.SubmitCommand.Execute(null);
        }

        private void IFSCCodeBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                SubmitForm();
            }
        }


        private void NameBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.SelectionStart = sender.Text.Length;
        }
    }
}
