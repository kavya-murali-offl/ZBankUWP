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
using ZBank.Entities.BusinessObjects;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class ViewTransaction : UserControl
    {
        public ViewTransaction()
        {
            this.InitializeComponent();
        }

        public TransactionBObj InViewTransaction
        {
            get { return (TransactionBObj)GetValue(InViewTransactionProperty); }
            set { 
                
                SetValue(InViewTransactionProperty, value); }
        }

        public static readonly DependencyProperty InViewTransactionProperty =
            DependencyProperty.Register("InViewTransaction", typeof(TransactionBObj), typeof(ViewTransaction), new PropertyMetadata(null));

        private void CloseRightPaneButton_Click(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.OnRightPaneContentUpdated(null);
        }
    }
}
