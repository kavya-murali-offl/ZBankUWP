using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ViewModel
{
    public class CardsViewModel : ViewModelBase
    {

        public IView View;

        public CardsViewModel(IView view)
        {
            View = view;
            LoadAllCards();
        }
        public void OnPageLoaded()
        {
            ViewNotifier.Instance.CardsPageDataUpdated += UpdateCardsList;
            LoadAllCards();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsPageDataUpdated -= UpdateCardsList;
        }


        private ObservableCollection<Card> AllCards
        {
            get { return _allCards; }
            set { _allCards = value; OnPropertyChanged(nameof(AllCards)); }
        }

        private ObservableCollection<Card> _allCards { get; set; }


        public void LoadAllCards()
        {
            GetAllCardsRequest request = new GetAllCardsRequest()
            {
                CustomerID = "1111"
            };

            IPresenterCallback<GetAllCardsResponse> presenterCallback = new GetAllCardsPresenterCallback(this);
            UseCaseBase<GetAllCardsResponse> useCase = new GetAllCardsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateCardsList(CardPageDataUpdatedArgs args)
        {
            AllCards = new ObservableCollection<Card>(args.CardsList);
        }
    }
}
