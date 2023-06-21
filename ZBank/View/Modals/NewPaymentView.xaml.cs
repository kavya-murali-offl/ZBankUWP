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
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View.DataTemplates.NewPaymentTemplates;
using ZBank.ViewModel;
using ZBank.ViewModel.VMObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.Modals
{
    public sealed partial class NewPaymentView : UserControl, IView
    {

        private readonly TransferAmountViewModel ViewModel;

        public NewPaymentView()
        {
            this.InitializeComponent();
            ViewModel = new TransferAmountViewModel(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void CancelPayment_Click(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.OnCancelPaymentRequested(false);
        }

        //private void SwitchTemplate(int currentStep)
        //{
        //    switch (currentStep)
        //    {
        //        case 1:
        //            PrimaryButton.Command = ViewModel.OnNextCommand;
        //            PrimaryButton.Content = "Next";
        //            SecondaryButton.Command = ViewModel.OnResetCommand;
        //            SecondaryButton.Content = "Reset";
        //            NewPaymentContent.Content = ((DataTemplate)Resources["PaymentDetails"]).LoadContent();
        //            break;
        //        case 2:
        //            PrimaryButton.Command = ViewModel.OnProceedToPayCommand;
        //            PrimaryButton.Content = "Proceed To Pay";
        //            SecondaryButton.Command = ViewModel.OnBackCommand;
        //            SecondaryButton.Content = "Back";
        //            NewPaymentContent.Content = ((DataTemplate)Resources["PaymentConfirmation"]).LoadContent();
        //            break;
        //        case 3:
        //            NewPaymentContent.Content = ((DataTemplate)Resources["PaymentConfirmation"]).LoadContent();
        //            break;
        //        default:
        //            break;
        //    }
        //}


        private void PayButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
