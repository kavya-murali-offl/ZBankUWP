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
using static ZBankManagement.Domain.UseCase.InitializeApp;
using System.IO;

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
        public void OnLoaded()
        {
            InitializeAppRequest request = new InitializeAppRequest()
            {
            };

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

    }
}
