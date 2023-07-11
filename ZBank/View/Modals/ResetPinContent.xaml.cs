using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Entity.BusinessObjects;
using ZBank.ViewModel;
using ZBankManagement.AppEvents.AppEventArgs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class ResetPinContent : UserControl, IView
    {
        private ResetPinViewModel ViewModel { get; set; }

       public ResetPinContent(string cardNumber)
       {
            this.InitializeComponent();
            ViewModel = new ResetPinViewModel(this);
            ViewModel.CardNumber = cardNumber;
       }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SubmitForm();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.OnCloseDialog(); 
        }

        private void PinBox_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
        {
            ViewModel.NewPin = sender.Password;
            ViewModel.ErrorText = string.Empty;
        }

        private void PinBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.SubmitForm();
            }
        }
    }
}
