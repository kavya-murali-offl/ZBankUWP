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

        public NewPaymentView(TransferAmountViewModel viewModel = null)
        {
            this.InitializeComponent();
            if(viewModel != null )
            {
                ViewModel = viewModel;
            }
            else
            {
                ViewModel = new TransferAmountViewModel(this);
            }
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
            ViewModel.ResumeAtCurrentStep();
            ViewNotifier.Instance.OnCancelPaymentRequested(false);
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
