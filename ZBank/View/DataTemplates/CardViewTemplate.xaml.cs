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
using ZBank.Entity.Constants;
using ZBank.Utilities.Helpers;
using ZBank.View.Main;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class CardViewTemplate : UserControl
    {
       
        public decimal AvailableCreditLimit { get; set; }


        public Card SelectedCard
        {
            get { return (Card)GetValue(SelectedCardProperty); }
            set { SetValue(SelectedCardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCardProperty =
            DependencyProperty.Register("SelectedCard", typeof(Card), typeof(CardViewTemplate), new PropertyMetadata(null));


        public CardViewTemplate()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(SelectedCard != null) {
                if (SelectedCard is DebitCard)
                {
                    CardSpecificContent.Content = (Resources["DebitCardTemplate"] as DataTemplate).LoadContent();
                }
                else if (SelectedCard is CreditCard)
                {
                    CreditCard creditCard = SelectedCard as CreditCard;
                    AvailableCreditLimit = (creditCard.CreditLimit - creditCard.TotalOutstanding);
                    CardSpecificContent.Content = (Resources["CreditCardTemplate"] as DataTemplate).LoadContent();
                }
            }
        }
    }
}
