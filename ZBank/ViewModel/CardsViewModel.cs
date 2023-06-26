using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Devices.PointOfService;
using Windows.UI.Core;
using Windows.UI.Popups;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.Constants;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using static ZBankManagement.Domain.UseCase.ResetPin;

namespace ZBank.ViewModel
{
    public class CardsViewModel : ViewModelBase
    {

        public IView View;

        public ICommand PreviousCardCommand { get; set; }

        public ICommand NextCardCommand { get; set; }

        public CardsViewModel(IView view)
        {
            View = view;
            DataModel = new CardPageModel();
            PreviousCardCommand = new RelayCommand(GoToPreviousCard, () =>
            {
                if(AllCards?.Count > 0)
                {
                    return AllCards.ElementAtOrDefault(DataModel.OnViewCardIndex - 1) != null;
                }
                return false;
            });
            NextCardCommand = new RelayCommand(GoToNextCard, () =>
            {
                if(AllCards?.Count > 0)
                {
                     return AllCards.ElementAtOrDefault(DataModel.OnViewCardIndex + 1) != null;
                }
                return false;
            });
        }

        private void GoToNextCard(object obj)
        {
            UpdateView(DataModel.OnViewCardIndex + 1);
        }

        private void GoToPreviousCard(object obj)
        {
            UpdateView(DataModel.OnViewCardIndex - 1);
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


        private ObservableCollection<CardBObj> _allCards { get; set; }

        public ObservableCollection<CardBObj> AllCards
        {
            get { return _allCards; }
            set { 
                _allCards = value; 
                OnPropertyChanged(nameof(AllCards));
            }
        }

        private CardPageModel _dataModel { get; set; }

        public CardPageModel DataModel
        {
            get { return _dataModel; }
            set { 
                _dataModel = value;
                OnPropertyChanged(nameof(DataModel)); }
        }

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

        public void ResetPin()
        {
            ResetPinRequest request = new ResetPinRequest()
            {
                CardNumber = DataModel.OnViewCard.CardNumber,
                NewPin = "1111"
            };

            IPresenterCallback<ResetPinResponse> presenterCallback = new ResetPinPresenterCallback(this);
            UseCaseBase<ResetPinResponse> useCase = new ResetPinUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateCardsList(CardDataUpdatedArgs args)
        {
            int index = 0;
            foreach (var card in args.CardsList)
            {
                if (index > Constants.CardBackgrounds.Count)
                {
                    index = 0;
                }
                card.BackgroundImage = Constants.CardBackgrounds[index];
                index++;
            }
            AllCards = new ObservableCollection<CardBObj>(args.CardsList);
            UpdateView(0);
        }

        private void UpdateView(int onViewIndex)
        {
            DataModel = new CardPageModel()
            {

                OnViewCardIndex = onViewIndex,
                OnViewCard = AllCards.ElementAtOrDefault(onViewIndex),
                LeftCard = AllCards.ElementAtOrDefault(onViewIndex - 1),
                RightCard = AllCards.ElementAtOrDefault(onViewIndex + 1)
            };
            (NextCardCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCardCommand as RelayCommand).RaiseCanExecuteChanged();
        }


        private class GetAllCardsPresenterCallback : IPresenterCallback<GetAllCardsResponse>
        {
            private readonly CardsViewModel ViewModel;

            public GetAllCardsPresenterCallback(CardsViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllCardsResponse response)
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

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = exception.Message,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

        private class ResetPinPresenterCallback : IPresenterCallback<ResetPinResponse>
        {
            private CardsViewModel ViewModel { get; set; }

            public ResetPinPresenterCallback(CardsViewModel cardsViewModel)
            {
                ViewModel = cardsViewModel;
            }

            public async Task OnSuccess(ResetPinResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    // Dialog open
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = response.Message,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });

            }
        }
    }

    public class CardPageModel
    {
        public CardBObj LeftCard { get; set; }

        public CardBObj RightCard { get; set; }

        public CardBObj OnViewCard { get; set; }

        public int OnViewCardIndex { get; set; }

    }
}
