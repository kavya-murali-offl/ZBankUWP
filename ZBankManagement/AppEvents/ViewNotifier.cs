﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.AppEvents.AppEventArgs;
using ZBank.Entities;
using ZBankManagement.AppEvents.AppEventArgs;

namespace ZBank.AppEvents
{
    public class ViewNotifier
    {

        public event Action LoadApp;
        public void OnLoadApp()
        {
            LoadApp?.Invoke();
        }

        public bool PaymentInProgress { get; set; }

        public event Action<bool> RequestFailed;
        public void OnRequestFailed(bool close)
        {
            RequestFailed?.Invoke(close);
        }

        public event Action<bool> CloseDialog;
        public void OnCloseDialog(bool close)
        {
            CloseDialog?.Invoke(close);
        }

        public event Action<Beneficiary> BeneficiaryRemoved;
        public void OnBeneficiaryRemoved(Beneficiary beneficiary)
        {
            BeneficiaryRemoved?.Invoke(beneficiary);
        }


        public event Action<Beneficiary, bool> BeneficiaryAddOrUpdated;
        public void OnBeneficiaryAddOrUpdated(Beneficiary beneficiary, bool isAdded)
        {
            BeneficiaryAddOrUpdated?.Invoke(beneficiary, isAdded);
        }

        public event Action<int> CurrentPaymentStepChanged;
        public void OnCurrentPaymentStepChanged(int step)
        {
            CurrentPaymentStepChanged?.Invoke(step);
        }

        public event Action<bool> CancelPaymentRequested;
        public void OnCancelPaymentRequested(bool isPaymentCompleted)
        {
            CancelPaymentRequested?.Invoke(isPaymentCompleted);
        }

        public event Action<bool> AccountInserted;
        public void OnAccountInserted (bool isSuccess)
        {
            AccountInserted?.Invoke(isSuccess);
        }

        public event Action<NotifyUserArgs> NotificationStackUpdated;
        public void OnNotificationStackUpdated(NotifyUserArgs args)
        {
            NotificationStackUpdated?.Invoke(args);
        }

        public event Action<BranchListUpdatedArgs> BranchListUpdated;
        public void OnBranchListUpdated(BranchListUpdatedArgs args)
        {
            BranchListUpdated?.Invoke(args);
        }

        public event Action<BeneficiaryListUpdatedArgs> BeneficiaryListUpdated;
        public void OnBeneficiaryListUpdated(BeneficiaryListUpdatedArgs args)
        {
            BeneficiaryListUpdated?.Invoke(args);
        }

        public event Action<CardDataUpdatedArgs> CardsDataUpdated;
        public void OnCardsPageDataUpdated(CardDataUpdatedArgs args)
        {
            CardsDataUpdated?.Invoke(args);
        }

        public event Action<TransactionPageDataUpdatedArgs> TransactionListUpdated;
        public void OnTransactionsListUpdated(TransactionPageDataUpdatedArgs args)
        {
            TransactionListUpdated?.Invoke(args);
        }

        public event Action<FrameContentChangedArgs> FrameContentChanged;
        public void OnFrameContentChanged(FrameContentChangedArgs args)
        {
            FrameContentChanged?.Invoke(args);
        }


        public event Action<DashboardDataUpdatedArgs> DashboardDataChanged;
        public void OnDashboardDataChanged(DashboardDataUpdatedArgs args)
        {
            DashboardDataChanged?.Invoke(args);
        }


        public event Action<bool> LoggedIn;
        public void OnSuccessfulLogin(bool IsLoginSuccess)
        {
            LoggedIn?.Invoke(IsLoginSuccess);
        }

        public event Action<AccountsListUpdatedArgs> AccountsListUpdated;
        public void OnAccountsListUpdated(AccountsListUpdatedArgs args)
        {
            AccountsListUpdated?.Invoke(args);
        }

        public event Action<ElementTheme> ThemeChanged;
        public void OnThemeChanged(ElementTheme theme)
        {
            ThemeChanged?.Invoke(theme);
        }

        private ViewNotifier() { }

        private static ViewNotifier instance = null;

        public static ViewNotifier Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ViewNotifier();
                }
                return instance;
            }
        }
    }
}
