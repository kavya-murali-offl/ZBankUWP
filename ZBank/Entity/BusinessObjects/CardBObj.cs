using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.Constants;
using ZBank.Utilities.Helpers;

namespace ZBank.Entities.BusinessObjects
{
   
    public class CardBObj
    {

        public readonly IList<string> _cardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.jpg",
            "/Assets/CardBackgrounds/card2.jpg",
            "/Assets/CardBackgrounds/card3.jpg",
            "/Assets/CardBackgrounds/card4.jpg",
        };

        public static int bgIndex = 0;

        public void SetDefaultValues()
        {
            if (bgIndex >= _cardBackgrounds.Count)
            {
                bgIndex = 0;
            }

            BackgroundImage = _cardBackgrounds[bgIndex];
            bgIndex++;

            if (Type == CardType.DEBIT)
            {
                TypeString = "DEBIT";
                CustomTextKey = "";
                CustomTextValue = "";
                ProviderLogo = Constants.ZBankLogo;
            }
            else if (Type == CardType.CREDIT)
            {
                TypeString = "CREDIT";
                CustomTextKey = "Available Credit Limit";
                CustomTextValue = (CreditLimit - TotalOutstanding);
                ProviderLogo = LogoHelper.GetCardProviderPath(CreditCardProvider);
            }
        }

        public string AccountNumber { get; set; }

        public string CardNumber { get; set; }

        public string CustomTextKey { get; set; }

        public decimal CreditLimit { get; set; }

        public decimal TotalOutstanding { get; set; }

        public CreditCardProvider CreditCardProvider { get; set; }

        public string TypeString { get; set; }
        public CardType Type { get; set; }

        public decimal MinimumOutstanding { get; set; }

        public decimal Interest { get; set; }


        public object CustomTextValue { get; set; }

        public string BackgroundImage { get; set; }

        public string ProviderLogo { get; set; }

    }
}
