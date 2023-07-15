using System;
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
using System.IO;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBankManagement.AppEvents.AppEventArgs;
using ZBank.DataStore;
using Microsoft.Toolkit.Uwp;
using ZBankManagement.Entity.BusinessObjects;

namespace ZBank.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IList<Navigation> TopNavigationList { get; private set; }

        private string _title = string.Empty;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private Navigation _selectedItem;

        public Navigation SelectedItem
        {
            get { return _selectedItem; }
            set =>  Set(ref _selectedItem, value);  
        }

        private Customer _currentCustomer;

        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set => Set(ref _currentCustomer, value);
        }
        public void OnLoaded()
        {
            GetCustomerData();
            ViewNotifier.Instance.FrameContentChanged += ChangeFrame;
            ViewNotifier.Instance.GetCustomerSuccess += CustomerFetched;
        }

        private void ChangeFrame(FrameContentChangedArgs args)
        {
            Title = args.Title;
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

        public MainViewModel(IView view)
        {
            View = view;
            TopNavigationList = new List<Navigation>
            {
                new Navigation("Dashboard", "Dashboard".GetLocalized(), "\uE80F", typeof(DashboardPage)),
                new Navigation("Accounts", "Accounts".GetLocalized(), "\uE910", typeof(AccountsPage), typeof(AccountInfoPage)),
                new Navigation("Cards", "Cards".GetLocalized(), "\uE8C7", typeof(CardsPage)),
                new Navigation("Transactions", "Transactions".GetLocalized(), "\uE8AB", typeof(TransactionsPage)),
                new Navigation("Beneficiaries", "Beneficiaries".GetLocalized(), "\uE716", typeof(BeneficiariesPage)),
            };

            SelectedItem = TopNavigationList.FirstOrDefault();
            Title = SelectedItem.Text.GetLocalized();
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
                Params = pageParams,
                Title = navigation.Text
            };

            Title = navigation.Text;
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
            ViewNotifier.Instance.FrameContentChanged -= ChangeFrame;
        }

        internal async Task OpenSettingsWindow()
        {
            await WindowService.ShowOrSwitchAsync<SettingsPage>(false);
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
                await ViewModel.View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnGetCustomerSuccess(response.Customer);
                });
            }

            public async Task OnFailure(ZBankException error)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(
                        new Notification()
                        {
                            Message = error.Message,
                            Type = NotificationType.ERROR
                    });
                });
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
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                        ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                        {
                            Message = "Signout Successful",
                            Duration = 3000,
                            Type = NotificationType.SUCCESS
                        });
                        ViewNotifier.Instance.OnCurrentUserChanged(null);
                });
            }

            public async Task OnFailure(ZBankException response)
            {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnNotificationStackUpdated(new Notification()
                    {
                        Message = "Signout failed",
                        Duration = 3000,
                        Type = NotificationType.SUCCESS
                    });
                });
            }
        }

    }

    public class MainPageArgs
    {
        public string CustomerID { get; set; }
    }
}
