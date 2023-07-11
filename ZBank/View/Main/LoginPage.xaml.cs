using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page, IView
    {
        private LoginViewModel ViewModel { get; set; }  

        public LoginPage()
        {
            this.InitializeComponent();
            ViewModel = new LoginViewModel(this);
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnUnloaded();
        }


        private void PasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorText = string.Empty;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.ErrorText = string.Empty;
        }

        private void PasswordText_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                ViewModel.LoginCommand.Execute(null);   
            }
        }
    }
}
