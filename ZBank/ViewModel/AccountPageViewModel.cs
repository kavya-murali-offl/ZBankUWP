using ZBankManagement.Domain.UseCase;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZBank.Entities;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities.EnumerationType;
using ZBank.Entities.BusinessObjects;
using Windows.UI.Core;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using ZBankManagement.AppEvents.AppEventArgs;
using Windows.UI.Xaml.Controls.Primitives;
using ZBank.DataStore;
using ZBank.Services;

namespace ZBank.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {
        public IView View;

        public AccountPageViewModel(IView view)
        {
            View = view;
        }

        public void OnPageLoaded()
        {
            ViewNotifier.Instance.AccountsListUpdated += UpdateAccountsList;
            ViewNotifier.Instance.AccountInserted += OnAccountInserted;
            LoadAllAccounts();
        }

        private void OnAccountInserted(bool obj)
        {
            LoadAllAccounts();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
            ViewNotifier.Instance.AccountInserted -= OnAccountInserted;
        }

        private ObservableCollection<Account> _accounts = null;

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => Set(ref _accounts, value);
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                IsTransactionAccounts = false,
                AccountType = null,
                UserID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetAllAccountsResponse> presenterCallback = new GetAllAccountsPresenterCallback(this);
            UseCaseBase<GetAllAccountsResponse> useCase = new GetAllAccountsUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void UpdateAccountsList(AccountsListUpdatedArgs args)
        {
            Accounts = new ObservableCollection<Account>(args.AccountsList);
        }

        private class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public GetAllAccountsPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AccountsListUpdatedArgs args = new AccountsListUpdatedArgs()
                    {
                        AccountsList = new ObservableCollection<AccountBObj>(response.Accounts)
                    };
                    ViewNotifier.Instance.OnAccountsListUpdated(args);
                   
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
