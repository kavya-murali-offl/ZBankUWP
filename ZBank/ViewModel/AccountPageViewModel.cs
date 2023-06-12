﻿using ZBankManagement.Domain.UseCase;
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
            LoadAllAccounts();
        }

        public void OnPageUnLoaded()
        {
            ViewNotifier.Instance.AccountsListUpdated -= UpdateAccountsList;
        }

        public void GoToAccountInfoPage(object parameter)
        {
            var paras = parameter;
        }
        public void UpdateNotification(NotifyUserArgs args)
        {

        }

        public void InsertAccount(object parameter)
        {
            InsertAccountRequest request = new InsertAccountRequest()
            {
               //AccountToInsert = new SavingsAccount()
               //{
               //     AccountNumber = "test2",
               //     IFSCCode = "ZBNK1233",
               //     AccountName = "xxxxxxx",
               //     AccountStatus = AccountStatus.ACTIVE,
               //     OpenedOn = DateTime.Now,
               //     Currency = Currency.INR,
               //     Amount = 100,
               //     UserID="1111"
               //}
            };
            
            //IPresenterCallback<InsertAccountResponse> presenterCallback = new InsertAccountPresenterCallback(this);

            //UseCaseBase<InsertAccountResponse> useCase = new InsertAccountUseCase(request, presenterCallback);
            //useCase.Execute();
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

        public void LoadAllAccounts()
        {
            GetAllAccountsRequest request = new GetAllAccountsRequest()
            {
                AccountType = null,
                UserID = "1111"
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
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public UpdateAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(UpdateAccountResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                });
            }

            public async Task OnFailure(ZBankException response)
            {
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
                        AccountsList = new ObservableCollection<Account>(response.Accounts)
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
                        Exception = response
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });

            }


        }
    }





}
