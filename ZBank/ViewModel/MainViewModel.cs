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

        public MainViewModel(IView view)
        {
            View = view;
            TopNavigationList = new List<Navigation>
            {
                new Navigation("Dashboard", "\uE80F", typeof(DashboardPage)),
                new Navigation("Accounts", "\uE910", typeof(AccountsPage), typeof(AccountInfoPage)),
                new Navigation("Cards", "\uE8C7", typeof(CardsPage)),
                new Navigation("Transactions", "\uE8AB", typeof(TransactionsPage)),
            };

            SelectedItem = TopNavigationList.FirstOrDefault();
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
                default:
                    return typeof(DashboardPage);
            }
        }
    }
}
