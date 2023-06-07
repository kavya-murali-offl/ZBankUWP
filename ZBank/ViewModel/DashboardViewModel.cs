using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
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
    public class DashboardViewModel : ViewModelBase
    {
        public IView View;

        private CardBObj _onViewCard { get; set; }

        private int _onViewCardIndex { get; set; } = 0;

        public CardBObj OnViewCard
        {
            get { return _onViewCard; }
            set
            {
                _onViewCard = value;
                OnPropertyChanged(nameof(OnViewCard));
            }
        }

        public void OnLoaded()
        {
            ViewNotifier.Instance.DashboardDataChanged += RefreshData;
            LoadContent();
        }

        public void UpdateOnViewCard()
        {
            if(_onViewCardIndex < 0 || _onViewCardIndex >= DashboardModel.AllCards.Count)
            {
                _onViewCardIndex = 0;
            }
            OnViewCard = DashboardModel.AllCards.ElementAt(_onViewCardIndex);
        }

        public void OnNextCard()
        {
            _onViewCardIndex++;
            if(_onViewCardIndex >= DashboardModel.AllCards.Count)
            {
                _onViewCardIndex = 0;
            }
            UpdateOnViewCard();
        }

        public void OnPreviousCard()
        {
            _onViewCardIndex--;
            if (_onViewCardIndex < 0)
            {
                _onViewCardIndex = DashboardModel.AllCards.Count - 1;
            }
            UpdateOnViewCard();
        }

        private DashboardDataModel model;

        public DashboardDataModel DashboardModel
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged(nameof(DashboardModel));   
            }
        }

        public void RefreshData(DashboardDataUpdatedArgs args)
        {
           foreach (var transactionBObj in args.LatestTransactions)
           { 
               transactionBObj.SetDefault();
           }
           foreach(var card in args.AllCards)
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
                UserID = "1111"
            };

            IPresenterCallback<GetDashboardDataResponse> presenterCallback = new GetDashboardDataPresenterCallback(this);
            UseCaseBase<GetDashboardDataResponse> useCase = new GetDashboardDataUseCase(request, presenterCallback);
            useCase.Execute();
        }


        public void OnUnLoaded()
        {
            ViewNotifier.Instance.DashboardDataChanged -= RefreshData;
        }

        public DashboardViewModel(IView view)
        {
            View = view;
        }

     
    }
    public class GetAllAccountsInDashboardPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
    {
        private DashboardViewModel DashboardViewModel { get; set; }

        public GetAllAccountsInDashboardPresenterCallback(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel = dashboardViewModel;
        }

        public async void OnSuccess(GetAllAccountsResponse response)
        {
            await DashboardViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                {
                    AccountsList = new ObservableCollection<Account>(response.Accounts)
                };
                ViewNotifier.Instance.OnAccountsListUpdated(args);
            });
        }

        public void OnFailure(ZBankException response)
        {
            // Notify view
        }
    }


    public class GetDashboardDataPresenterCallback : IPresenterCallback<GetDashboardDataResponse>
    {
        private DashboardViewModel DashboardViewModel { get; set; }

        public GetDashboardDataPresenterCallback(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel = dashboardViewModel;
        }

        public async void OnSuccess(GetDashboardDataResponse response)
        {
            await DashboardViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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

        public void OnFailure(ZBankException response)
        {

        }
    }

    public class GetAllBeneficiariesInDashboardPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
    {

        private readonly DashboardViewModel _dashboardViewModel;

        public void OnSuccess(GetAllBeneficiariesResponse response)
        {
        }

        public void OnFailure(ZBankException response)
        {
        }
    }

}
