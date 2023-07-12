using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.Services;
using ZBank.View;
using ZBank.View.Main;
using ZBank.View.Modals;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Helpers;

namespace ZBank.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        public ICommand PreviousCardCommand { get; set; }
        public ICommand NextCardCommand { get; set; }


        public DashboardViewModel(IView view)
        {
            View = view;
            Title = "Dashboard";
            PreviousCardCommand = new RelayCommand(OnPreviousCard, 
                () => DashboardModel?.AllCards?.Count > 0 ? DashboardModel?.AllCards?.ElementAtOrDefault(_onViewCardIndex - 1) != null : false);
            
            NextCardCommand = new RelayCommand(OnNextCard,
                () => DashboardModel?.AllCards?.Count > 0 ? DashboardModel?.AllCards?.ElementAtOrDefault(_onViewCardIndex + 1) != null : false);
        }


        public void OnLoaded()
        {
            ViewNotifier.Instance.DashboardDataChanged += RefreshData;
            ViewNotifier.Instance.CancelPaymentRequested += PaymentCompleted;
            ViewNotifier.Instance.CardInserted += OnCardInserted;
            LoadContent();
        }

        private void OnCardInserted(bool IsInserted, Card insertedCard)
        {
            if(IsInserted)
            {
                LoadContent();
            }
        }

        private void PaymentCompleted(bool isCompleted)
        {
            if(isCompleted) LoadContent();
        }

        public void UpdateOnViewCard()
        {
            if (DashboardModel.AllCards.Count > 0)
            {
                if (_onViewCardIndex < 0 || _onViewCardIndex >= DashboardModel.AllCards.Count)
                {
                    _onViewCardIndex = 0;
                }
                OnViewCard = DashboardModel.AllCards.ElementAt(_onViewCardIndex);
            };

            (NextCardCommand as RelayCommand).RaiseCanExecuteChanged();
            (PreviousCardCommand as RelayCommand).RaiseCanExecuteChanged();
        }

        public void OnNextCard(object parameter = null)
        {
            _onViewCardIndex++;
            if (_onViewCardIndex >= DashboardModel.AllCards.Count)
            {
                _onViewCardIndex = 0;
            }
            UpdateOnViewCard();
        }

        public void OnPreviousCard(object parameter = null)
        {
            _onViewCardIndex--;
            if (_onViewCardIndex < 0)
            {
                _onViewCardIndex = DashboardModel.AllCards.Count - 1;
            }
            UpdateOnViewCard();
        }

        private DashboardDataModel _dashboardModel;

        public DashboardDataModel DashboardModel
        {
            get { return _dashboardModel; }
            set => Set(ref _dashboardModel, value);
        }

        private CardBObj _onViewCard = null;

        public CardBObj OnViewCard
        {
            get => _onViewCard;
            set => Set(ref _onViewCard, value);
        }
        
        private int _onViewCardIndex { get; set; } = -1;


        public void RefreshData(DashboardDataUpdatedArgs args)
        {
            foreach (var card in args.AllCards)
            {
                card.SetDefaultValues();
            }

            DashboardModel = new DashboardDataModel()
            {
                AllBeneficiaries = new ObservableCollection<Beneficiary>(args.AllBeneficiaries),
                AllCards = new ObservableCollection<CardBObj>(args.AllCards),
                LatestTransactions = new ObservableCollection<TransactionBObj>(args.LatestTransactions),
                BalanceCard = args.BalanceCard,
                DepositCard = args.DepositCard,
                IncomeExpenseCard = args.IncomeExpenseCard,
                BeneficiariesCard = args.BeneficiariesCard,
                AllAccounts = new ObservableCollection<AccountBObj>(args.AllAccounts)
            };

            _onViewCardIndex = 0;
            UpdateOnViewCard();
        }

        public void LoadContent()
        {
            GetDashboardDataRequest request = new GetDashboardDataRequest()
            {
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetDashboardDataResponse> presenterCallback = new GetDashboardDataPresenterCallback(this);
            UseCaseBase<GetDashboardDataResponse> useCase = new GetDashboardDataUseCase(request, presenterCallback);
            useCase.Execute();
        }


        public void OnUnLoaded()
        {
            ViewNotifier.Instance.DashboardDataChanged -= RefreshData;
            ViewNotifier.Instance.CancelPaymentRequested -= PaymentCompleted;
            ViewNotifier.Instance.CardInserted -= OnCardInserted;
        }

        public void ManageCard()
        {
            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = typeof(CardsPage),
                Params = new CardsPageParams()
                {
                    OnViewCard = OnViewCard,
                },
                Title = "Cards".GetLocalized()
            };

            ViewNotifier.Instance.OnFrameContentChanged(args);
        }

        internal void OnViewMoreTransactions()
        {
            FrameContentChangedArgs args = new FrameContentChangedArgs();
            args.PageType = typeof(TransactionsPage);
            args.Title = "Transactions";
            ViewNotifier.Instance.OnFrameContentChanged(args);
        }

        internal async Task OpenAddCreditCardDialog()
        {
            await DialogService.ShowContentAsync(View, new AddCardView(), "New Credit Card", Window.Current.Content.XamlRoot);
        }

        private class GetDashboardDataPresenterCallback : IPresenterCallback<GetDashboardDataResponse>
        {
            private DashboardViewModel ViewModel { get; set; }

            public GetDashboardDataPresenterCallback(DashboardViewModel dashboardViewModel)
            {
                ViewModel = dashboardViewModel;
            }

            public async Task OnSuccess(GetDashboardDataResponse response)
            {

                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    DashboardDataUpdatedArgs args = new DashboardDataUpdatedArgs()
                    {
                        AllCards = response.AllCards,
                        DepositCard = response.DepositCard,
                        AllBeneficiaries = response.Beneficiaries,
                        BeneficiariesCard = response.BeneficiariesCard,
                        AllAccounts = response.Accounts,
                        BalanceCard = response.BalanceCard,
                        IncomeExpenseCard = response.IncomeExpenseCard,
                        LatestTransactions = response.LatestTransactions,
                    };

                    ViewNotifier.Instance.OnDashboardDataChanged(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
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
}
