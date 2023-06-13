using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.Entities.BusinessObjects;
using ZBank.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class AmountInfoCardUserControl : UserControl
    {
        public DashboardCardModel Card
        {
            get { return (DashboardCardModel)GetValue(CardProperty); }
            set { SetValue(CardProperty, value); }
        }

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(DashboardCardModel), typeof(AmountInfoCardUserControl), new PropertyMetadata(null));

        public AmountInfoCardUserControl()
        {
            this.InitializeComponent();
        }
    }
}
