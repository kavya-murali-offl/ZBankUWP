using System;
using System.Collections.Generic;
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
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.DashboardDataChanged += RefreshData;
            LoadAllAccounts();
            LoadContent();
        }

        public void UpdateOnViewCard()
        {
            if(_onViewCardIndex < 0 || _onViewCardIndex >= DashboardModel.AllCards.Count)
            {

            }
            else
            {
                OnViewCard = DashboardModel.AllCards.ElementAt(_onViewCardIndex);
            }
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
            DashboardModel = args.DashboardModel;
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
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.DashboardDataChanged -= RefreshData;
        }

        public void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            Accounts = args.AccountsList;
        }

        private ObservableCollection<Account> _accounts { get; set; }

        public void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                UserID = "1111"
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsInDashboardPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public DashboardViewModel(IView view)
        {
            this.View = view;
        }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }
    }


}
