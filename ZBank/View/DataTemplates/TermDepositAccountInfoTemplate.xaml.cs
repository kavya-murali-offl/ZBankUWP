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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.DataTemplates
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TermDepositAccountInfoTemplate : Page
    {
        public TermDepositAccountInfoTemplate()
        {
            this.InitializeComponent();
        }

        public TermDepositAccount SelectedAccount
        {
            get { return (TermDepositAccount)GetValue(SelectedAccountProperty); }
            set { SetValue(SelectedAccountProperty, value); }
        }

        public static readonly DependencyProperty SelectedAccountProperty =
            DependencyProperty.Register("SelectedAccount", typeof(TermDepositAccount), typeof(TermDepositAccountInfoTemplate), new PropertyMetadata(null));


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ViewOrNoCardTemplate.DataContext = this;

            //ViewOrNoCardTemplate.Content = ((DataTemplate)Resources["ViewCardTemplate"]).LoadContent();
        }
    }
}
