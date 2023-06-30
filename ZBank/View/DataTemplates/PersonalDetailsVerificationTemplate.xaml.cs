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
using ZBank.Entities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class PersonalDetailsVerificationTemplate : UserControl
    {
        public PersonalDetailsVerificationTemplate()
        {
            this.InitializeComponent();
        }

        public Customer CurrentCustomer
        {
            get { return (Customer)GetValue(CurrentCustomerProperty); }
            set { SetValue(CurrentCustomerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentCustomer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentCustomerProperty =
            DependencyProperty.Register("CurrentCustomer", typeof(Customer), typeof(PersonalDetailsVerificationTemplate), new PropertyMetadata(null));


    }
}
