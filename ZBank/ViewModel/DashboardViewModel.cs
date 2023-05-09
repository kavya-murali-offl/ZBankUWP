using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;

namespace ZBank.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        public IView View;

        private readonly IList<string> _cardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.webp",
            "/Assets/CardBackgrounds/card2.webp",
            "/Assets/CardBackgrounds/card3.webp",
            "/Assets/CardBackgrounds/card4.webp",
        };


        private ObservableCollection<TransactionBObj> _transactions { get; set; }

        private ObservableCollection<CardBObj> _cards { get; set; }

        private ObservableCollection<Account> _accounts { get; set; }

        private ObservableCollection<Beneficiary> _beneficiaries { get; set; }

        public DashboardViewModel(IView view)
        {
            this.View = view;

            _transactions = new ObservableCollection<TransactionBObj>
            {
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 11000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Paytm", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 1000, DateTime.Now, 3000, "Income", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 4000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Flipkart", "Shopping")
            };

            _cards = new ObservableCollection<CardBObj>
            {
               new CardBObj("11111111", CardType.DEBIT, _cardBackgrounds.ElementAt(0)),
               new CardBObj("22222222", CardType.CREDIT, _cardBackgrounds.ElementAt(1)),
               new CardBObj("33333333", CardType.DEBIT, _cardBackgrounds.ElementAt(2)),
               new CardBObj("44444444", CardType.CREDIT, _cardBackgrounds.ElementAt(3)),
            };
        }

        public ObservableCollection<TransactionBObj> LatestTransactions
        {
            get { return _transactions; }
            set { 
                _transactions = value; 
                OnPropertyChanged(nameof(LatestTransactions));
            }
        }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ObservableCollection<Beneficiary> Beneficiaries
        {
            get { return _beneficiaries; }
            set
            {
                _beneficiaries = value;
                OnPropertyChanged(nameof(Beneficiaries));
            }
        }

        public ObservableCollection<CardBObj> AllCards
        {
            get { return _cards; }
            set { _cards = value; OnPropertyChanged(nameof(AllCards)); }
        }
    }
}
