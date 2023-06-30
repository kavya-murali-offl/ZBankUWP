﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.Config;
using ZBank.View.Main;
using ZBank.View;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using ZBank.View.UserControls;
using ZBank.Services;
using ZBank.ZBankManagement.DomainLayer.UseCase;
using ZBankManagement.Domain.UseCase;
using static ZBankManagement.Domain.UseCase.InitializeApp;
using System.IO;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBank.DataStore;

namespace ZBank.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IList<Navigation> TopNavigationList { get; private set; }
        public IView View;
        private Navigation _selectedItem;

        public Navigation SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private Customer _currentCustomer;

        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                _currentCustomer = value;
                OnPropertyChanged(nameof(CurrentCustomer));
            }
        }
        public void OnLoaded()
        {
            InitializeAppData();
            GetCustomerData();
            ViewNotifier.Instance.GetCustomerSuccess += CustomerFetched;
        }

        private void CustomerFetched(Customer obj)
        {
            CurrentCustomer = obj;
        }

        private void GetCustomerData()
        {
            GetCustomerRequest request = new GetCustomerRequest()
            {
                CustomerID = Repository.Current.CurrentUserID
            };

            IPresenterCallback<GetCustomerResponse> presenterCallback = new GetCustomerPresenterCallback(this);
            UseCaseBase<GetCustomerResponse> useCase = new GetCustomerUseCase(request, presenterCallback);
            useCase.Execute();
        }

        private void InitializeAppData()
        {
            InitializeAppRequest request = new InitializeAppRequest();

            IPresenterCallback<InitializeAppResponse> presenterCallback = new InitializeAppPresenterCallback(this);
            UseCaseBase<InitializeAppResponse> useCase = new InitializeAppUseCase(request, presenterCallback);
            useCase.Execute();
        }

        public MainViewModel(IView view)
        {
            LoadWindow();
            View = view;
            TopNavigationList = new List<Navigation>
            {
                new Navigation("Dashboard", "\uE80F", typeof(DashboardPage)),
                new Navigation("Accounts", "\uE910", typeof(AccountsPage), typeof(AccountInfoPage)),
                new Navigation("Cards", "\uE8C7", typeof(CardsPage)),
                new Navigation("Transactions", "\uE8AB", typeof(TransactionsPage)),
                new Navigation("Beneficiaries", "\uE716", typeof(BeneficiariesPage)),
            };

            SelectedItem = TopNavigationList.FirstOrDefault();
        }
        private async void LoadWindow()
        {
            await WindowManagerService.Current.InitializeAsync();
        }

        public void UpdateSelectedPage(Type pageType)
        {
            SelectedItem = TopNavigationList.Where(item => item.PageTypes.Contains(pageType)).FirstOrDefault();
        }

        public void NavigationChanged(Navigation navigation)
        {
            SelectedItem = navigation;
            object pageParams = null;
            Type pageType = GetPageType(SelectedItem.Tag);

            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = pageType,
                Params = pageParams
            };
            ViewNotifier.Instance.OnFrameContentChanged(args);
        }

        public Type GetPageType(string tag)
        {
            switch (tag)
            {
                case "Dashboard":
                    return typeof(DashboardPage);
                case "Accounts":
                    return typeof(AccountsPage);
                case "Cards":
                    return typeof(CardsPage);
                case "Transactions":
                    return typeof(TransactionsPage);
                case "Beneficiaries":
                    return typeof(BeneficiariesPage);
                default:
                    return typeof(DashboardPage);
            }
        }

        public void Signout()
        {
            LogoutCustomerRequest request = new LogoutCustomerRequest()
            {
                CustomerID = AppSettings.Current.CustomerID,
            };

            IPresenterCallback<LogoutCustomerResponse> presenterCallback = new LogoutUserPresenterCallback(this);
            UseCaseBase<LogoutCustomerResponse> useCase = new LogoutCustomerUseCase(request, presenterCallback);
            useCase.Execute();
        }

        internal void OnUnloaded()
        {
            ViewNotifier.Instance.GetCustomerSuccess -= CustomerFetched;
        }

        private class GetCustomerPresenterCallback : IPresenterCallback<GetCustomerResponse>
        {
            private MainViewModel ViewModel { get; set; }

            public GetCustomerPresenterCallback(MainViewModel accountPageViewModel)
            {
                ViewModel = accountPageViewModel;
            }

            public async Task OnSuccess(GetCustomerResponse response)
            {
                await ViewModel.View.Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //ViewNotifier.Instance.OnGetCustomerSuccess(response.Customer);
                });
            }

            public async Task OnFailure(ZBankException error)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = error.Message,
                            Type = NotificationType.ERROR
                        }
                    });
                });
            }
        }


        private class InitializeAppPresenterCallback : IPresenterCallback<InitializeAppResponse>
        {
            public MainViewModel ViewModel { get; set; }

            public InitializeAppPresenterCallback(MainViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(InitializeAppResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewNotifier.Instance.OnLoadApp();
                });
            }

            public async Task OnFailure(ZBankException response)
            {

            }
        }

        private class LogoutUserPresenterCallback : IPresenterCallback<LogoutCustomerResponse>
        {
            public MainViewModel ViewModel { get; set; }

            public LogoutUserPresenterCallback(MainViewModel viewModel)
            {
                ViewModel = viewModel;
            }

            public async Task OnSuccess(LogoutCustomerResponse response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                        NotifyUserArgs args = new NotifyUserArgs()
                        {
                            Notification = new Notification()
                            {
                                Message = "Signout Successful",
                                Duration = 3000,
                                Type = NotificationType.SUCCESS
                            }
                        };
                        ViewNotifier.Instance.OnNotificationStackUpdated(args);
                        ViewNotifier.Instance.OnCurrentUserChanged(null);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await ViewModel.View.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    NotifyUserArgs args = new NotifyUserArgs()
                    {
                        Notification = new Notification()
                        {
                            Message = "Signout failed",
                            Duration = 3000,
                            Type = NotificationType.SUCCESS
                        }
                    };
                    ViewNotifier.Instance.OnNotificationStackUpdated(args);
                });
            }
        }

    }

    public class MainPageArgs
    {
        public string CustomerID { get; set; }
    }
}
