﻿using System;
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
using ZBank.Entity.BusinessObjects;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.DataTemplates
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SavingsAccountInfoTemplate : UserControl
    {
        public SavingsAccountInfoTemplate()
        {
            this.InitializeComponent();
        }

        public SavingsAccount SelectedAccount
        {
            get { return (SavingsAccount)GetValue(SelectedAccountProperty); }
            set { SetValue(SelectedAccountProperty, value); }
        }

        public static readonly DependencyProperty SelectedAccountProperty =
            DependencyProperty.Register("SelectedAccount", typeof(SavingsAccount), typeof(SavingsAccountInfoTemplate), new PropertyMetadata(null));

        public CardBObj LinkedCard
        {
            get { return (CardBObj)GetValue(LinkedCardProperty); }
            set { SetValue(LinkedCardProperty, value); }
        }

        public static readonly DependencyProperty LinkedCardProperty =
            DependencyProperty.Register("LinkedCard", typeof(CardBObj), typeof(CurrentAccountInfoTemplate), new PropertyMetadata(null));

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

}
