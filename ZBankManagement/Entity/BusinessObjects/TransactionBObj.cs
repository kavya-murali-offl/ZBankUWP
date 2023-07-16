﻿using System;
using Windows.UI.Xaml.Media;
using ZBank.Entities;

namespace ZBank.Entities.BusinessObjects
{
    public class TransactionBObj : Transaction
    {

        public string BeneficiaryName { get; set; } 

        public string ExternalName { get; set; } 

        public string AccountName { get; set; } 
        
        public decimal ClosingBalance { get; set; }    

        public string Name {
            get =>
                TransactionType == TransactionType.SELF_TRANSFER ? "Me" :
                TransactionType == TransactionType.CARD_PAYMENT ? "Card Payment" :
                (!string.IsNullOrEmpty(BeneficiaryName) ? BeneficiaryName :
                (!string.IsNullOrEmpty(AccountName) ? AccountName :
                (!string.IsNullOrEmpty(ExternalName) ? ExternalName : "You")));
        }

        public string SenderName 
        { 
            get => IsRecipient ? Name : "Me";
        }

        public string Recipient
        { 
            get => IsRecipient ? "Me" : Name; 
        }

        public bool IsRecipient { get; set; }   

        public string ArrowIcon 
        { 
            get =>  IsRecipient ? "\uEDDB" : "\uEDDC";
        }

        public string PlusOrMinus
        { 
            get =>  IsRecipient ? "+" : "-";
        }

        public string BorderColor { 
            get =>  IsRecipient ? "#058365" :  "#BE3232";
        }

        public string BackgroundColor
        {
            get => IsRecipient ? "#EAFDE8" : "#F5E1DD";
        }
    }
}
