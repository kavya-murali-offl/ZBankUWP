﻿using ZBankManagement.Domain.UseCase;
using ZBankManagement.Interface;
using System;
using System.Linq;
using Windows.UI.Core;
using ZBank.Dependencies;
using ZBank.Entities;
using ZBank.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace ZBank.ZBankManagement.DomainLayer.UseCase
{
        public class InsertAccountUseCase : UseCaseBase<InsertAccountResponse>
        {
            private readonly IInsertAccountDataManager _insertAccountDataManager = DependencyContainer.ServiceProvider.GetRequiredService<IInsertAccountDataManager>();
            private readonly IPresenterCallback<InsertAccountResponse> _presenterCallback;
            private readonly InsertAccountRequest _request;

            public InsertAccountUseCase(InsertAccountRequest request, IPresenterCallback<InsertAccountResponse> presenterCallback)
            {
                _presenterCallback = presenterCallback;
                _request = request;
            }

            protected override void Action()
            {
                _insertAccountDataManager.InsertAccount(_request, new InsertAccountCallback(this));
            }

            private class InsertAccountCallback : IUseCaseCallback<InsertAccountResponse>
            {

                private InsertAccountUseCase _useCase;

                public InsertAccountCallback(InsertAccountUseCase useCase)
                {
                    _useCase = useCase;
                }

                public void OnSuccess(InsertAccountResponse response)
                {
                    _useCase._presenterCallback.OnSuccess(response);
                }

                public void OnFailure(ZBankError error)
                {
                    _useCase._presenterCallback.OnFailure(error);
                }
            }
        }

        public class InsertAccountRequest
        {
            public Account AccountToInsert { get; set; }
        }

        public class InsertAccountResponse
        {
            public bool IsSuccess { get; set; }

            public Account InsertedAccount { get; set; }
        }

        public class InsertAccountPresenterCallback : IPresenterCallback<InsertAccountResponse>
        {
            private AccountPageViewModel AccountPageViewModel { get; set; }

            public InsertAccountPresenterCallback(AccountPageViewModel accountPageViewModel)
            {
                AccountPageViewModel = accountPageViewModel;
            }

            public async void OnSuccess(InsertAccountResponse response)
            {
                await AccountPageViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (AccountPageViewModel.Accounts != null)
                    {
                        var accounts = AccountPageViewModel.Accounts.Prepend(response.InsertedAccount);
                        AccountPageViewModel.Accounts = new ObservableCollection<Account>(accounts);
                    }
                });
            }

            public void OnFailure(ZBankError error)
            {
                
            }
        }
}
