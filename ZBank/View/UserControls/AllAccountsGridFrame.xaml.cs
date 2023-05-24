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
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllAccountsGridFrame : Page, IView
    {

        public AllAccountsGridFrame()
        {
            this.InitializeComponent();
            ViewModel = new AccountPageViewModel(this);
        }

        public AccountPageViewModel ViewModel { get; private set; }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnPageUnLoaded();
        }
    }
}