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
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.Entity.Constants;
using ZBank.Services;
using ZBank.View;
using ZBank.View.UserControls;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using static ZBankManagement.Domain.UseCase.ResetPin;

namespace ZBank.ViewModel
{
    public class CardsViewModel : ViewModelBase
    {

        public IView View;

        public CardsPageParams Params { get; set; } 

        public ICommand PreviousCardCommand { get; set; }
        public ICommand NextCardCommand { get; set; }
        public ICommand ResetPinCommand { get; set; }

        public CardsViewModel(IView view)
        {
            View = view;
            ResetPinCommand = new RelayCommand(ResetPin);
            PreviousCardCommand = new RelayCommand(GoToPreviousCard,
                () => DataModel?.AllCards?.Count() > 0 ? DataModel?.AllCards?.ElementAtOrDefault(DataModel.OnViewCardIndex - 1) != null : false);
            
            NextCardCommand = new RelayCommand(GoToNextCard, () =>
                DataModel?.AllCards?.Count() > 0 ? DataModel?.AllCards?.ElementAtOrDefault(DataModel.OnViewCardIndex + 1) != null : false);
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
            ViewNotifier.Instance.CardInserted += OnCardInserted;
            LoadAllCards();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCardsList;
            ViewNotifier.Instance.CardInserted -= OnCardInserted;
        }

        private void OnCardInserted(bool arg1, Card arg2)
        {
            LoadAllCards();
        }

        private CardPageModel _dataModel { get; set; } = new CardPageModel();

        public CardPageModel DataModel
        {
            get { return _dataModel; }
            set {
                _dataModel = value;
                OnPropertyChanged(nameof(DataModel));   
            }
        }

        public void LoadAllCards()
        {
            GetAllCardsRequest request = new GetAllCardsRequest()
            {
                CustomerID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllCardsResponse> presenterCallback = new GetAllCardsPresenterCallback(this);
            UseCaseBase<GetAllCardsResponse> useCase = new GetAllCardsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public void ResetPin(object parameter)
        {
            ResetPinArgs args = (ResetPinArgs)parameter;
            if (args != null)
            {
                ResetPinRequest request = new ResetPinRequest()
                {
                    CardNumber = args.CardNumber,
                    NewPin = args.PinNumber
                };

                IPresenterCallback<ResetPinResponse> presenterCallback = new ResetPinPresenterCallback(this);
                UseCaseBase<ResetPinResponse> useCase = new ResetPinUseCase(request, presenterCallback);
                useCase.Execute();
            }
        }

        private void UpdateCardsList(CardDataUpdatedArgs args)
        {
            int index = 0;
            foreach (var card in args.CardsList)
            {
                if (index >= Constants.CardBackgrounds.Count)
                {
                    index = 0;
                }
                card.BackgroundImage = Constants.CardBackgrounds[index];
                index++;
                card.SetDefaultValues();
            }
            
            DataModel = new CardPageModel();
            DataModel.AllCards = args.CardsList.Count() > 0 ? new ObservableCollection<CardBObj>(args.CardsList) : null;
            
            if (Params?.OnViewCard?.CardNumber != null)
            {
                CardBObj cardBOBj = args.CardsList.FirstOrDefault(card => card.CardNumber == Params.OnViewCard.CardNumber);
                int onViewCardIndex = args.CardsList.ToList().IndexOf(cardBOBj);
                UpdateView(onViewCardIndex); 
            }
            else
            {
                UpdateView(0);
            }
        }

        private void UpdateView(int onViewIndex)
        {
            DataModel.OnViewCardIndex = onViewIndex;
            OnPropertyChanged(nameof(DataModel));
            (NextCardCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCardCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        internal void UpdateTransactionLimit(double value)
        {
            
            UpdateCardRequest request = new UpdateCardRequest()
            {
                CustomerID = Repository.Current.CurrentUserID,
                CardToUpdate = DataModel.OnViewCard,
            };

            request.CardToUpdate.TransactionLimit = decimal.Parse(value.ToString());

            IPresenterCallback<UpdateCardResponse> presenterCallback = new UpdateLimitPresenterCallback(this);
            UseCaseBase<UpdateCardResponse> useCase = new UpdateCardUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class UpdateLimitPresenterCallback : IPresenterCallback<UpdateCardResponse>
        {
            private CardsViewModel ViewModel { get; set; }

            public UpdateLimitPresenterCallback(CardsViewModel cardsViewModel)
            {
                ViewModel = cardsViewModel;
            }

            public async Task OnSuccess(UpdateCardResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnUpdatedLimit(true, response.UpdatedCard);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Transaction Limit Update Successful",
                        Type = NotificationType.SUCCESS
                    });
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnUpdatedLimit(false, null);
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = response.Message,
                        Type = NotificationType.ERROR
                    });
                });

            }
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
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = exception.Message,
                        Type = NotificationType.ERROR
                    });
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Reset Pin Successful",
                        Type = NotificationType.SUCCESS
                    });
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(()=>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                        {
                            Message = response.Message,
                            Type = NotificationType.ERROR
                        });
                });

            }
        }
    }

    public class CardPageModel
    {
        public CardBObj LeftCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex - 1); }

        public CardBObj RightCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex + 1); }

        public CardBObj OnViewCard { get => AllCards?.ElementAtOrDefault(OnViewCardIndex); }
      
        public bool IsOnViewCreditCard { get => OnViewCard?.Type == CardType.CREDIT; }
       
        public CreditCard OnViewCreditCard { get => OnViewCard is CreditCard ? OnViewCard as CreditCard : null; }
      
        public DebitCard OnViewDebitCard { get => OnViewCard is DebitCard ? OnViewCard as DebitCard : null; }

        public int OnViewCardIndex {  get; set; }

        public int TotalCreditCards { get => AllCards?.Where(card => card.Type == CardType.CREDIT).Count() ?? 0; }

        public int TotalDebitCards { get => AllCards?.Where(card => card.Type == CardType.DEBIT).Count() ?? 0; }

        public int TotalAllCards { get => AllCards?.Count() ?? 0; }

        public IEnumerable<CardBObj> AllCards { get; set; }

        public int MaximumCards = 3;
    }
}
