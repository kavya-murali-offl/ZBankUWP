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
using ZBank.Entity.Constants;
using ZBank.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class CardElementView : UserControl
    {
        public CardElementView()
        {
            this.InitializeComponent();
        }

        public bool IsCreditCard { get; set; }  

        public CardBObj TemplateCard
        {
            get { return (CardBObj)GetValue(TemplateCardProperty); }
            set { 
                SetValue(TemplateCardProperty, value); }
        }

        public string BankLogo { get; set; } = Constants.ZBankLogo;
        public static readonly DependencyProperty TemplateCardProperty =
            DependencyProperty.Register("TemplateCard", typeof(CardBObj), typeof(CardElementView), new PropertyMetadata(null));
    }
}
