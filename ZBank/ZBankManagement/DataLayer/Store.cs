﻿using ZBank.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZBankManagement.Data
{
    public class Store
    {
        public static Customer CurrentUser { get; set; }

        public static IEnumerable<Card> CardsList { get; set; }

        public static IEnumerable<Account> AccountsList { get; set; }

        public static IEnumerable<Transaction> TransactionsList { get; set; }


        // AccountsList

        public static Account GetAccountByAccountNumber(string accountNumber) => AccountsList.Where(acc => acc.AccountNumber.Equals(accountNumber)).FirstOrDefault();

        // CardsList
         //&& c.AccountID.Equals(accountID)
        public static bool IsDebitCardEnabled(string accountID) => CardsList.Where(c => c.Type.Equals(CardType.DEBIT)).Any();

        public static bool IsCreditCardEnabled() => CardsList.Where(c => c.Type.Equals(CardType.CREDIT)).Any();

        public static bool IsCardNumber(string cardNumber) => CardsList.Where<Card>(card => card.CardNumber == cardNumber).Any();

        public static Card GetCard(string cardNumber) => CardsList.Where<Card>(card => card.CardNumber == cardNumber).FirstOrDefault();


        public static IEnumerable<Card> GetCardsList()
        {
            return CardsList;
        }

        public static bool IsCreditCard(string cardNumber) => CardsList.Where(c => c.Type == CardType.CREDIT && c.CardNumber == cardNumber).Any();

        public static bool IsDebitCardLinked(string accountID) => CardsList.Where(card => card.Pin == accountID).Any();
    }
}
