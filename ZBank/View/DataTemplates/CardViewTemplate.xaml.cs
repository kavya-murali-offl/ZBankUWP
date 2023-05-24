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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.DataTemplates
{
    public sealed partial class CardViewTemplate : UserControl
    {
        public Card Card { get; set; }
        public string BackgroundImage { get; set; }
        public string ProviderLogo { get; set; }
        public decimal AvailableCreditLimit { get; set; }

        public CardViewTemplate()
        {
            this.InitializeComponent();
            Card = DataContext as Card;  
            if(Card != null)
                 SetDefaultValues();
        }

        public readonly IList<string> _cardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.jpg",
            "/Assets/CardBackgrounds/card2.jpg",
            "/Assets/CardBackgrounds/card3.jpg",
            "/Assets/CardBackgrounds/card4.jpg",
        };

        public int bgIndex = 0;

        public void SetDefaultValues()
        {
            if (bgIndex >= _cardBackgrounds.Count)
            {
                bgIndex = 0;
            }

            BackgroundImage = _cardBackgrounds[0];
            bgIndex++;
            if (Card == null)
            {
                //CardSpecificTemplate.Content = this.Resources["NoCardsToShow"] as DataTemplate;
                //CardSpecificTemplate.DataContext = null;
            }
            else { 
                if (Card.Type == CardType.DEBIT)
                {
                    DebitCard debitCard = Card as DebitCard;
                    ProviderLogo = LogoHelper.GetCardProviderPath();
                }
                else if (Card.Type == CardType.CREDIT)
                {
                    CreditCard creditCard = Card as CreditCard;
                    ProviderLogo = LogoHelper.GetCardProviderPath(creditCard.CreditCardProvider);
                    AvailableCreditLimit = (creditCard.CreditLimit - creditCard.TotalOutstanding);
                    CardSpecificTemplate.Content = this.Resources["CreditCardTemplate"] as DataTemplate;
                    CardSpecificTemplate.DataContext = creditCard;
                }
            }
           
        }
    }
}
