using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;
using ZBank.Entity.BusinessObjects;

namespace ZBankManagement.Entity.BusinessObjects
{
    public class CardPageModel
    {

            public CardBObj LeftCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex - 1); }

            public CardBObj RightCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex + 1); }

            public CardBObj OnViewCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex); }
            
            public bool IsOnViewCreditCard { get => OnViewCard?.Type == CardType.CREDIT; }

            public CreditCard OnViewCreditCard { get => OnViewCard is CreditCard ? OnViewCard as CreditCard : null; }

            public DebitCard OnViewDebitCard { get => OnViewCard is DebitCard ? OnViewCard as DebitCard : null; }

            public int OnViewCardIndex { get; set; }

            public int TotalCreditCards { get => AllCards?.Where(card => card.Type == CardType.CREDIT).Count() ?? 0; }

            public int TotalDebitCards { get => AllCards?.Where(card => card.Type == CardType.DEBIT).Count() ?? 0; }

            public int TotalAllCards { get => AllCards?.Count() ?? 0; }

            public IEnumerable<CardBObj> AllCards { get; set; }

            public int MaximumCards = 3;
        }
}
