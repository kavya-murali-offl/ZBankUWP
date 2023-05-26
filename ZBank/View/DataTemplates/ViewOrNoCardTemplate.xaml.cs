using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class ViewOrNoCardTemplate : UserControl
    {
        public ViewOrNoCardTemplate()
        {
            this.InitializeComponent();
            UpdateCard();
        }
        public string BackgroundImage { get; set; } = Constants.CardBackgrounds.FirstOrDefault();

        public Card SelectedCard
        {
            get { return (Card)GetValue(SelectedCardProperty); }
            set { 
                
                SetValue(SelectedCardProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedCardProperty =
            DependencyProperty.Register("SelectedCard", typeof(Card), typeof(ViewOrNoCardTemplate), new PropertyMetadata(null));

        public string ProviderLogo { get; set; } = Constants.ZBankLogo;

        public int bgIndex = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCard();
        }

        public void UpdateCard()
        {
            if (bgIndex >= Constants.CardBackgrounds.Count)
            {
                bgIndex = 0;
            }

            BackgroundImage = Constants.CardBackgrounds[0];
            bgIndex++;

            ViewOrNoCardContent.DataContext = this;
            if (SelectedCard != null)
            {
                if (SelectedCard is CreditCard)
                {
                    var selectedCreditCard = SelectedCard as CreditCard;
                    ProviderLogo = LogoHelper.GetCardProviderPath(selectedCreditCard.CreditCardProvider);
                }
                else
                {
                    ProviderLogo = LogoHelper.GetCardProviderPath();
                }
                ViewOrNoCardContent.Content = (Resources["ViewCardTemplate"] as DataTemplate).LoadContent();
            }
            else
            {
                ViewOrNoCardContent.Content = (Resources["NoCardTemplate"] as DataTemplate).LoadContent();
            }
        }
    }
}
