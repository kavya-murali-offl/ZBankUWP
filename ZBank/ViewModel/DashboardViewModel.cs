using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entity.BusinessObjects;
using ZBank.Model;

namespace ZBank.ViewModel
{
    public class DashboardViewModel
    {

        private IList<string> _cardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.webp",
            "/Assets/CardBackgrounds/card2.webp",
            "/Assets/CardBackgrounds/card3.webp",
            "/Assets/CardBackgrounds/card4.webp",
        };

        private ObservableCollection<TransactionBObj> _transactions { get; set; }

        private ObservableCollection<CardBObj> _cards { get; set; }

        public DashboardViewModel()
        {

            _transactions = new ObservableCollection<TransactionBObj>
            {
                new TransactionBObj(TransactionType.INCOME, ModeOfPayment.DIRECT, 11000, DateTime.Now, 3000, "Flipkart", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 2000, DateTime.Now, 3000, "Paytm", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 1000, DateTime.Now, 3000, "Income", "Shopping"),
                new TransactionBObj(TransactionType.EXPENSE, ModeOfPayment.DIRECT, 4000, DateTime.Now, 3000, "Flipkart", "Shopping"),
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
            set { _transactions = value; OnPropertyChanged("ID"); }
        }

        public ObservableCollection<CardBObj> AllCards
        {
            get { return _cards; }
            set { _cards = value; OnPropertyChanged("ID"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
