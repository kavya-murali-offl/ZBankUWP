using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
using ZBank.Utilities.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class ViewOrNoCardTemplate : UserControl
    {
        public ViewOrNoCardTemplate()
        {
            this.InitializeComponent();
        }

        public CardBObj SelectedCard
        {
            get { return (CardBObj)GetValue(SelectedCardProperty); }
            set { 
                SetValue(SelectedCardProperty, value);
                UpdateCard();
            }
        }

        public static readonly DependencyProperty SelectedCardProperty =
            DependencyProperty.Register("SelectedCard", typeof(CardBObj), typeof(ViewOrNoCardTemplate), new PropertyMetadata(null));

        public ICommand LinkCardCommand
        {
            get { return (ICommand)GetValue(LinkCardCommandProperty); }
            set { SetValue(LinkCardCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LinkCardCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkCardCommandProperty =
            DependencyProperty.Register("LinkCardCommand", typeof(ICommand), typeof(ViewOrNoCardTemplate), new PropertyMetadata(null));

        public string BankLogo { get; set; } = Constants.ZBankLogo;


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void UpdateCard()
        {
            ViewOrNoCardContent.DataContext = this;

            if (SelectedCard != null)
            {
                ViewOrNoCardContent.Content = (Resources["ViewCardTemplate"] as DataTemplate).LoadContent();
            }
            else
            {
                ViewOrNoCardContent.Content = (Resources["NoCardTemplate"] as DataTemplate).LoadContent();
            }
        }

        private void LinkCard_Click(object sender, RoutedEventArgs e)
        {
             LinkCardCommand.Execute(null);
        }
    }
}
