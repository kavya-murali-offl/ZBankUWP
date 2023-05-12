﻿using ZBankManagement.Domain.UseCase;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZBank.Entities;
using ZBank.View;
using ZBank.ViewModel.VMObjects;
using ZBank.ZBankManagement.DomainLayer.UseCase;

namespace ZBank.ViewModel
{
    public class AccountPageViewModel : ViewModelBase
    {
        public IView View;

        public ICommand AddAccountCommand { get; set; } 

        public AccountPageViewModel(IView view)
        {
            View = view;
            LoadAllAccounts();
            AddAccountCommand = new RelayCommand(InsertAccount);
        }

        public void InsertAccount(object parameter)
        {
            InsertAccountRequest request = new InsertAccountRequest()
            {
               AccountToInsert = new CurrentAccount()
               {
                    AccountNumber = "test1",
                    IFSCCode = "ZBNK1233",
                    AccountName = "xxxxxxx",
                    AccountStatus = AccountStatus.ACTIVE,
                    OpenedOn = DateTime.Now,
                    Currency = Currency.INR,
                    Amount = 100,
                    UserID="1111"
               }
            };
            
            IPresenterCallback<InsertAccountResponse> presenterCallback = new InsertAccountPresenterCallback(this);

            UseCaseBase<InsertAccountResponse> useCase = new InsertAccountUseCase(request, presenterCallback);
            useCase.Execute();
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
    }
}
