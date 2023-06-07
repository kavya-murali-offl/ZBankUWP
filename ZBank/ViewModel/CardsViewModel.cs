using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
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
            ViewNotifier.Instance.CardsDataUpdated += UpdateCardsList;
            LoadAllCards();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCardsList;
        }


        private ObservableCollection<CardBObj> AllCards
        {
            get { return _allCards; }
            set { _allCards = value; OnPropertyChanged(nameof(AllCards)); }
        }

        private ObservableCollection<CardBObj> _allCards { get; set; }


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

        private void UpdateCardsList(CardDataUpdatedArgs args)
        {
            AllCards = new ObservableCollection<CardBObj>(args.CardsList);
        }
    }

    public class GetAllCardsPresenterCallback : IPresenterCallback<GetAllCardsResponse>
    {
        private readonly CardsViewModel ViewModel;

        public GetAllCardsPresenterCallback(CardsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public async void OnSuccess(GetAllCardsResponse response)
        {
            await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CardDataUpdatedArgs args = new CardDataUpdatedArgs()
                {
                    CardsList = response.Cards
                };
                ViewNotifier.Instance.OnCardsPageDataUpdated(args);
            });
        }

        public async void OnFailure(ZBankException exception)
        {
            var dialog = new MessageDialog(exception.Message);
            await dialog.ShowAsync();

        }
    }
}
