using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZBankManagement.AppEvents.AppEventArgs;
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

        private IEnumerable<TransactionBObj> _transactions { get; set; } = new List<TransactionBObj>();

        public IEnumerable<TransactionBObj> Transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                LoadCardByID();
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public AccountInfoViewModel(IView view)
        {
            View = view;
        }

        public void OnPageLoaded()
        {

            ViewNotifier.Instance.CardsDataUpdated += UpdateCard;
            ViewNotifier.Instance.TransactionListUpdated += UpdateTransactions;
            LoadTransactions();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCard;
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactions;
        }


        public void UpdateCard(CardDataUpdatedArgs args)
        {
            LinkedCard = args.CardsList.FirstOrDefault();
            if (LinkedCard != null)
            {
                LinkedCard.SetDefaultValues();
            }
        }

        public void UpdateTransactions(TransactionPageDataUpdatedArgs args)
        {
            Transactions = args.TransactionList;
            if (Transactions != null)
            {
                foreach (var transaction in Transactions)
                {
                    transaction.SetDefault();
                }
            }
        }


        private void LoadTransactions()
        {

            GetAllTransactionsRequest request = new GetAllTransactionsRequest()
            {
                AccountNumber = SelectedAccount.AccountNumber,
            };

            IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsOfAccountPresenterCallback(this);
            UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void LoadCardByID()
        {

            GetAllCardsRequest request = new GetAllCardsRequest()
            {
                CustomerID = null,
                CardNumber = SelectedAccount.CardNumber
            };

            IPresenterCallback<GetAllCardsResponse> presenterCallback = new GetCardByNumberInAccountPresenterCallback(this);
            UseCaseBase<GetAllCardsResponse> useCase = new GetAllCardsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private class GetCardByNumberInAccountPresenterCallback : IPresenterCallback<GetAllCardsResponse>
        {
            private readonly AccountInfoViewModel ViewModel;

            public GetCardByNumberInAccountPresenterCallback(AccountInfoViewModel viewModel)
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
                var dialog = new MessageDialog(exception.Message);
                await dialog.ShowAsync();

            }
        }

        private class GetAllTransactionsOfAccountPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
        {
            public AccountInfoViewModel ViewModel { get; set; }

            public GetAllTransactionsOfAccountPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllTransactionsResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                    {
                        TransactionList = response.Transactions,
                    };

                    ViewNotifier.Instance.OnTransactionsListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
            }
        }

        private class GetAllBeneficiariesInAccountPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
        {
            public AccountInfoViewModel ViewModel { get; set; }

            public GetAllBeneficiariesInAccountPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllBeneficiariesResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    BeneficiaryListUpdatedArgs args = new BeneficiaryListUpdatedArgs()
                    {
                        BeneficiaryList = response.Beneficiaries
                    };
                    ViewNotifier.Instance.OnBeneficiaryListUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
            }
        }
    }
}
