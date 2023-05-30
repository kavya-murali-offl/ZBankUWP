using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entity.BusinessObjects;
using ZBank.View;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using ZBankManagement.Utility;

namespace ZBank.ViewModel
{
    public class AccountInfoViewModel : ViewModelBase
    {
        public IView View;

        private AccountBObj _selectedAccount { get; set; }

        public AccountBObj SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        private CardBObj _linkedCard { get; set; }

        public CardBObj LinkedCard
        {
            get { return _linkedCard; }
            set
            {
                _linkedCard = value;
                OnPropertyChanged(nameof(LinkedCard));
            }
        }

        public AccountInfoViewModel(IView view)
        {
            View = view;

        }

        public void OnPageLoaded()
        {
            LoadCardByID();
            ViewNotifier.Instance.CardsDataUpdated += UpdateCard;
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCard;
        }

        private static int bgIndex = 0;

        public void UpdateCard(CardDataUpdatedArgs args)
        {
            LinkedCard = args.CardsList.FirstOrDefault();
            if (LinkedCard != null)
            {
                LinkedCard.SetDefaultValues();
            }
        }

        private void LoadCardByID() {

                GetAllCardsRequest request = new GetAllCardsRequest()
                {
                    CustomerID = null,
                    CardNumber = SelectedAccount.CardNumber
                };

                IPresenterCallback<GetAllCardsResponse> presenterCallback = new GetCardByNumberInAccountPresenterCallback(this);
                UseCaseBase<GetAllCardsResponse> useCase = new GetAllCardsUseCase(request, presenterCallback);
                useCase.Execute();
        }
    }
}
