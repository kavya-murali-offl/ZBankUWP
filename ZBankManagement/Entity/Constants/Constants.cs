﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entity.Constants
{
    public class Constants
    {
        public static string MastercardLogo = "/Assets/CardLogo/mastercard.png";
        public static string VisaLogo = "/Assets/CardLogo/visa.png";
        public static string RupayLogo = "/Assets/CardLogo/rupay.jpg";
        public static string ZBankLogo = "/Assets/CardLogo/banklogo.png";
        public static int MaximumCreditCards = 5;

        public static readonly IList<string> CardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.jpg",
            "/Assets/CardBackgrounds/card2.jpg",
            "/Assets/CardBackgrounds/card3.jpg",
            "/Assets/CardBackgrounds/card4.jpg",
        };

    }
}
