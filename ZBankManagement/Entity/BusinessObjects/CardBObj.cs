using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.Constants;
using ZBank.Utilities.Helpers;
using ZBankManagement.Helpers;

namespace ZBank.Entity.BusinessObjects
{
    public class CardBObj : Card
    {
        private static int bgIndex { get; set; } = 0;

        public object CustomText1Key { get; set; }

        public object CustomText1Value { get; set; }

        public string BackgroundImage { get; set; } =   string.Empty;

        public string ProviderLogo { get; set; } = null;

        public void SetDefaultValues()
        {
            if (Type == CardType.CREDIT)
            {
                CreditCard creditCard = this as CreditCard;
                CustomText1Key = "AvailableCreditLimit".GetLocalized();
                CustomText1Value = (decimal)(creditCard.CreditLimit - creditCard.TotalOutstanding);
                ProviderLogo = LogoHelper.GetCardProviderPath(creditCard.CreditCardProvider);
            }
            else if (Type == CardType.DEBIT)
            {
                DebitCard debitCard = this as DebitCard;
                CustomText1Key = "AccountNumber".GetLocalized();
                CustomText1Value = debitCard.AccountNumber;
                ProviderLogo = null;
            }

            if (bgIndex >= Constants.Constants.CardBackgrounds.Count || bgIndex < 0)
            {
                bgIndex = 0;
            }

            BackgroundImage = Constants.Constants.CardBackgrounds[bgIndex];
            bgIndex++;

        }

    }
}
