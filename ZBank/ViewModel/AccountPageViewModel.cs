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
using ZBank.View.UserControls;
using Microsoft.Toolkit.Uwp;

namespace ZBank.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {

        public AccountPageViewModel(IView view)
        {
            View = view;
            Title = "Accounts".GetLocalized();
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

        internal void NavigateToInfoPage(AccountBObj account)
        {
            AccountInfoPageParams parameters = new AccountInfoPageParams()
            {
                SelectedAccount = account,
            };
            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = typeof(AccountInfoPage),
                Params = parameters,
            };

            ViewNotifier.Instance.OnFrameContentChanged(args);
        }

        private class GetAllAccountsPresenterCallback : IPresenterCallback<GetAllAccountsResponse>
        {
            private AccountPageViewModel ViewModel { get; set; }

            public GetAllAccountsPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                ViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(GetAllAccountsResponse response)
            {
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
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
