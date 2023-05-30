using System;
using System.Collections.ObjectModel;
using System.Linq;
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


}
