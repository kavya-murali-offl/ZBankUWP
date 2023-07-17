using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.Devices.PointOfService;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
using ZBank.View.Modals;
using ZBank.View.UserControls;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Entity.BusinessObjects;
using static ZBank.ZBankManagement.DomainLayer.UseCase.UpdateCard;
using static ZBankManagement.Domain.UseCase.ResetPin;

namespace ZBank.ViewModel
{
    public class CardsViewModel : ViewModelBase
    {

        public CardsPageParams Params { get; set; } 

        public ICommand PreviousCardCommand { get; set; }
        public ICommand NextCardCommand { get; set; }
        public ICommand ResetPinCommand { get; set; }

        public CardsViewModel(IView view)
        {
            View = view;
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
            ViewNotifier.Instance.LimitUpdated += OnUpdatedLimit;
            LoadAllCards();
        }

        private async void OnUpdatedLimit(bool isUpdated, string message)
        {
            if(isUpdated)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Limit Updated Successfully",
                        Type = NotificationType.SUCCESS
                    });
                });
            }
            else
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCardsList;
            ViewNotifier.Instance.CardInserted -= OnCardInserted;
            ViewNotifier.Instance.CreditCardSettled -= OnCreditCardSettled;
        }

        private void OnCreditCardSettled(bool arg1, string arg2)
        {
            LoadAllCards();
        }

        private void OnCardInserted(bool arg1, Card arg2)
        {
            LoadAllCards();
        }

        private CardPageModel _dataModel { get; set; } = null;

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
            int onViewCardIndex = 0;
            if (DataModel == null)
            {
                DataModel = new CardPageModel();
                if (Params?.OnViewCard?.CardNumber != null)
                {
                    CardBObj cardBOBj = args.CardsList.FirstOrDefault(card => card.CardNumber == Params.OnViewCard.CardNumber);
                    onViewCardIndex = args.CardsList.ToList().IndexOf(cardBOBj);
                    UpdateView(onViewCardIndex);
                }
            }
            else
            {
                onViewCardIndex = DataModel.OnViewCardIndex;
            }
            DataModel.AllCards = args.CardsList.Count() > 0 ? new ObservableCollection<CardBObj>(args.CardsList) : null;
            UpdateView(onViewCardIndex);
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

        internal async Task OpenResetPinDialog(string cardNumber)
        {
            await DialogService.ShowContentAsync(View, new ResetPinContent(cardNumber), "Reset Pin", Window.Current.Content.XamlRoot);
        }

        internal async Task OpenAddCardDialog()
        {
            await DialogService.ShowContentAsync(View, new AddCardView(), "Add Credit Card", Window.Current.Content.XamlRoot);
        }

        internal async Task OpenPayCardDialog()
        {
            await DialogService.ShowContentAsync(View, new PayCreditCard(DataModel.OnViewCard as CreditCard), "Pay Credit Card", Window.Current.Content.XamlRoot);
        }

        internal void GoToAccount()
        {
            if(DataModel.OnViewCard is DebitCard debitCard)
            {
                AccountInfoPageParams parameters = new AccountInfoPageParams()
                {
                    AccountNumber = debitCard.AccountNumber,
                };
                FrameContentChangedArgs args = new FrameContentChangedArgs()
                {
                    PageType = typeof(AccountInfoPage),
                    Params = parameters,
                    Title = "Account Info".GetLocalized()
                };

                ViewNotifier.Instance.OnFrameContentChanged(args);
            }
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
                    ViewNotifier.Instance.OnUpdatedLimit(true, string.Empty);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnUpdatedLimit(false, response.Message);
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
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

 }
}
