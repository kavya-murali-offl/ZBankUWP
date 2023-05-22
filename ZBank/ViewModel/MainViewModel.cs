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
                new Navigation("Dashboard", "\uEA8A"),
                new Navigation("Accounts", "\uE910"),
                new Navigation("Cards", "\uE8C7"),
                new Navigation("Transactions", "\uE8AB"),
            };
            SelectedItem = TopNavigationList.FirstOrDefault();

        }

        public void UpdateSelectedPage(Type pageType)
        {
            SelectedItem = TopNavigationList.Where(list => list.PageType == pageType).FirstOrDefault();
        }

        public void NavigationChanged(Navigation navigation)
        {
            SelectedItem = navigation;
            Type pageType = null;
            object pageParams = null;

            if (SelectedItem.Tag == "Transactions")
            {
                pageType = typeof(TransactionsPage);
            }
            else if (SelectedItem.Tag == "Accounts")
            {
                pageType = typeof(AccountsPage);
            }
            else if (SelectedItem.Tag == "Cards")
            {
                pageType = typeof(AccountsPage);
            }
            else
            {
                pageType = typeof(DashboardPage);
            }
            FrameContentChangedArgs args = new FrameContentChangedArgs()
            {
                PageType = pageType,
                Params = pageParams
            };
            ViewNotifier.Instance.OnFrameContentChanged(args);
        }
    }
}
