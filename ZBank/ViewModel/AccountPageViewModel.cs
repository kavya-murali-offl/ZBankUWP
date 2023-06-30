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


        private ObservableCollection<Account> _accounts { get; set; }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = new ObservableCollection<Account>(value);
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
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

        private class UpdateAccountPresenterCallback : IPresenterCallback<UpdateAccountResponse>
        {
            private AccountPageViewModel ViewModel { get; set; }

            public UpdateAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                ViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(UpdateAccountResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = "Account updated successfully",
                            Type = NotificationType.SUCCESS
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }

            public async Task OnFailure(ZBankException exception)
            {

                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = exception.Message,
                            Type = NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

        private class InsertTransactionPresenterCallback : IPresenterCallback<InsertTransactionResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertTransactionPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(InsertTransactionResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                });
            }

            public async Task OnFailure(ZBankException response)
            {

            }
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
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message=response.Message,
                            Duration=3000,
                             Type= NotificationType.ERROR
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });

            }
        }
    }





}
