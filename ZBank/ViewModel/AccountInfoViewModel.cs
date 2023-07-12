using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.DataStore;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Entities.EnumerationType;
using ZBank.Entity.BusinessObjects;
using ZBank.Services;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBankManagement.Domain.UseCase;

namespace ZBank.ViewModel
{
    public class AccountInfoViewModel : ViewModelBase
    {

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
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public AccountInfoViewModel(IView view)
        {
            View = view;
            CloseAccountCommand = new RelayCommand(CloseAccount);
            UpdateAccountCommand = new RelayCommand(UpdateAccount);
            LinkCardCommand = new RelayCommand(LinkCard);
        }

        private void LinkCard(object obj)
        {
            if(SelectedAccount.AccountType != AccountType.TERM_DEPOSIT)
            {
                InsertCardRequest request = new InsertCardRequest()
                {
                    CardType = Entities.CardType.DEBIT,
                    CustomerID = Repository.Current.CurrentUserID,
                    AccountNumber = SelectedAccount.AccountNumber
                };

                IPresenterCallback<InsertCardResponse> presenterCallback = new InsertCardPresenterCallback(this);
                UseCaseBase<InsertCardResponse> useCase = new InsertCardUseCase(request, presenterCallback);
                useCase.Execute();
            }
        }

        private void UpdateAccount(object obj)
        {
            Account account = (Account)obj;
            if(account != null)
            {
                UpdateAccountRequest request = new UpdateAccountRequest()
                {
                    UpdatedAccount = account,
                };

                IPresenterCallback<UpdateAccountResponse> presenterCallback = new UpdateAccountPresenterCallback(this);
                UseCaseBase<UpdateAccountResponse> useCase = new UpdateAccountUseCase(request, presenterCallback);
                useCase.Execute();
            }
        }

        private void CloseAccount(object account)
        {
            TermDepositAccount termDepositAccount = (TermDepositAccount)account;
            if(termDepositAccount != null)
            {
                CloseTermDepositAccount(termDepositAccount);
            }
        }

        private void CloseTermDepositAccount(TermDepositAccount termDepositAccount)
        {
            CloseDepositRequest request = new CloseDepositRequest()
            {
               DepositAccount = termDepositAccount,
               CustomerID = Repository.Current.CurrentUserID,
            };

            IPresenterCallback<CloseDepositResponse> presenterCallback = new CloseDepositPresenterCallback(this);
            UseCaseBase<CloseDepositResponse> useCase = new CloseDepositUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public ICommand CloseAccountCommand { get; set; }  
        
        public ICommand UpdateAccountCommand { get; set; } 
        
        public ICommand LinkCardCommand { get; set; }   

        public void OnPageLoaded()
        {

            ViewNotifier.Instance.CardsDataUpdated += UpdateCard;
            ViewNotifier.Instance.TransactionListUpdated += UpdateTransactions;
            ViewNotifier.Instance.DepositClosed += DepositClosed;
            ViewNotifier.Instance.AccountUpdated += OnAccountUpdated;
            ViewNotifier.Instance.CardInserted += OnCardInserted;
            LoadTransactions();
        }

        private void OnAccountUpdated(bool isUpdated, AccountBObj obj)
        {
            SelectedAccount = isUpdated ? obj : SelectedAccount;
        }

        private void DepositClosed(TermDepositAccount depositAccount)
        {
            SelectedAccount = depositAccount;
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.CardsDataUpdated -= UpdateCard;
            ViewNotifier.Instance.TransactionListUpdated -= UpdateTransactions;
            ViewNotifier.Instance.DepositClosed -= DepositClosed;
            ViewNotifier.Instance.AccountUpdated -= OnAccountUpdated;
            ViewNotifier.Instance.CardInserted -= OnCardInserted;
        }

        private void OnCardInserted(bool isUpdated, Card insertedCard)
        {
            SelectedAccount.CardNumber = insertedCard.CardNumber;   
            LoadCardByID();
        }

        public void UpdateCard(CardDataUpdatedArgs args)
        {
            LinkedCard = args.CardsList.FirstOrDefault();
            LinkedCard?.SetDefaultValues();
        }

        public void UpdateTransactions(TransactionPageDataUpdatedArgs args)
        {
            Transactions = args.TransactionList;
        }


        private void LoadTransactions()
        {

            GetAllTransactionsRequest request = new GetAllTransactionsRequest()
            {
                AccountNumber = SelectedAccount.AccountNumber,
                CustomerID = Repository.Current.CurrentUserID,
                 CurrentPageIndex = 0,
              RowsPerPage = 20
            };

            IPresenterCallback<GetAllTransactionsResponse> presenterCallback = new GetAllTransactionsOfAccountPresenterCallback(this);
            UseCaseBase<GetAllTransactionsResponse> useCase = new GetAllTransactionsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void LoadCardByID()
        {
            GetAllCardsRequest request = new GetAllCardsRequest()
            {
                CustomerID = Repository.Current.CurrentUserID,
                AccountNumber = SelectedAccount.AccountNumber
            };

            IPresenterCallback<GetAllCardsResponse> presenterCallback = new GetCardByNumberInAccountPresenterCallback(this);
            UseCaseBase<GetAllCardsResponse> useCase = new GetAllCardsUseCase(request, presenterCallback);
            useCase.Execute();
        }


        private class InsertCardPresenterCallback : IPresenterCallback<InsertCardResponse>
        {
            private AccountInfoViewModel ViewModel { get; set; }

            public InsertCardPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InsertCardResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() => {
                    ViewNotifier.Instance.OnCardInserted(true, response.InsertedCard); 
                    ViewNotifier.Instance.OnNotificationStackUpdated(
                        new Notification() { 
                            Message = "Card linked successfully", 
                            Type = NotificationType.SUCCESS
                        }); 
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = exception.Message,
                        Type = NotificationType.ERROR
                    });
                });
            }
        }


        private class UpdateAccountPresenterCallback : IPresenterCallback<UpdateAccountResponse>
        {
            private readonly AccountInfoViewModel ViewModel;

            public UpdateAccountPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(UpdateAccountResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnAccountUpdated(true);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnAccountUpdated(false);
                });
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


        private class CloseDepositPresenterCallback : IPresenterCallback<CloseDepositResponse>
        {
            private readonly AccountInfoViewModel ViewModel;

            public CloseDepositPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(CloseDepositResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnDepositClosed(response.DepositAccount);
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

        private class GetCardByNumberInAccountPresenterCallback : IPresenterCallback<GetAllCardsResponse>
        {
            private readonly AccountInfoViewModel ViewModel;

            public GetCardByNumberInAccountPresenterCallback(AccountInfoViewModel viewModel)
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

        private class GetAllTransactionsOfAccountPresenterCallback : IPresenterCallback<GetAllTransactionsResponse>
        {
            public AccountInfoViewModel ViewModel { get; set; }

            public GetAllTransactionsOfAccountPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllTransactionsResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    TransactionPageDataUpdatedArgs args = new TransactionPageDataUpdatedArgs()
                    {
                        TransactionList = response.Transactions,
                    };

                    ViewNotifier.Instance.OnTransactionsListUpdated(args);
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

        private class GetAllBeneficiariesInAccountPresenterCallback : IPresenterCallback<GetAllBeneficiariesResponse>
        {
            public AccountInfoViewModel ViewModel { get; set; }

            public GetAllBeneficiariesInAccountPresenterCallback(AccountInfoViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(GetAllBeneficiariesResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    BeneficiaryListUpdatedArgs args = new BeneficiaryListUpdatedArgs()
                    {
                        BeneficiaryList = response.Beneficiaries
                    };
                    ViewNotifier.Instance.OnBeneficiaryListUpdated(args);
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
