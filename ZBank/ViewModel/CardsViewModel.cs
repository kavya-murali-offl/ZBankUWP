using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.ViewModel
{
    public class CardsViewModel
    {

        private ObservableCollection<Card> _cards { get; set; }

        public CardsViewModel() { 
        
        
        }

        private readonly IList<string> _cardBackgrounds = new List<string>
        {
            "/Assets/CardBackgrounds/card1.webp",
            "/Assets/CardBackgrounds/card2.webp",
            "/Assets/CardBackgrounds/card3.webp",
            "/Assets/CardBackgrounds/card4.webp",
        };


        public void LoadCards(){

           _cards = new ObservableCollection<Card>()
            {
                new Card("111111", CardType.DEBIT),
                new Card("222222", CardType.CREDIT),
                new Card("2333333", CardType.DEBIT),
            };

        }

        private ObservableCollection<Card> AllCards
        {
            get { return _cards; }
            set { _cards = value; OnPropertyChanged("_cards"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
