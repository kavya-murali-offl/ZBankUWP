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
using ZBank.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates.NewPaymentTemplates
{
    public sealed partial class PaymentConfirmation : UserControl
    {
        private TransferAmountViewModel ViewModel { get; set; }

        public PaymentConfirmation()
        {
            this.InitializeComponent();
            ViewModel = DataContext as TransferAmountViewModel;
            DataContextChanged += (s, e) =>
            {
                ViewModel = DataContext as TransferAmountViewModel;
                Bindings.Update();
            };
        }
    }
}
