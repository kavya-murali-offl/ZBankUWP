﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public AddEditBeneficiaryView()
        {
            this.InitializeComponent();
            ViewModel = new AddEditBeneficiaryViewModel(this);
            CancelButton.Visibility = Visibility.Collapsed;
            BeneficiaryButton.Content = ViewModel.BeneficiaryTypes.ElementAt(0).ToString().GetLocalized();
        }

        public AddEditBeneficiaryView(BeneficiaryBObj beneficiaryBObj)
        {
            this.InitializeComponent();
            CancelButton.Visibility = Visibility.Visible;
            ViewModel = new AddEditBeneficiaryViewModel(this, beneficiaryBObj);
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
            ViewModel.OnLoaded();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnloaded();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.OnCloseDialog();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Reset(ViewModel.SelectedBeneficiary.BeneficiaryType);
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
    }
}
