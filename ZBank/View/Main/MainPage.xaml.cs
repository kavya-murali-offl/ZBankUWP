using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.Config;
using ZBank.View;
using Windows.UI;
using Windows.UI.ViewManagement;
using System.Net.NetworkInformation;
using Windows.ApplicationModel.Core;
using ZBank.View.Main;
using Windows.ApplicationModel.Store;
using ZBank.ViewModel;
using ZBank.ViewModel.VMObjects;
using System.Windows.Input;
using Windows.Media.Devices;
using System.ComponentModel;
using ZBankManagement.Events;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZBank
{

    public sealed partial class MainPage : Page
    {

        public IList<Navigation> TopNavigationList { get; private set; }

        public IList<Navigation> BottomNavigationList { get; private set; }

        public Navigation SelectedItem;

        public MainViewModel ViewModel;


        public MainPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        private void LoadTheme()
        {
            ThemeSelector.InitializeTheme();
            ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = ThemeSelector.Theme;
        }

        private void LoadData()
        {
            TopNavigationList = new List<Navigation>
            {
                new Navigation("Dashboard", "\uEA8A"),
                new Navigation("Accounts", "\uE910"),
                new Navigation("Cards", "\uE8C7"),
                new Navigation("Transactions", "\uE8AB"),
            };
            BottomNavigationList = new List<Navigation>
            {
                new Navigation("Switch Theme", ThemeSelector.GetIcon()),
                new Navigation("Settings", "\uE713"),
            };
        }

        private void SwitchIcon()
        {
            BottomNavigationList.ElementAt(0).IconSource = ThemeSelector.GetIcon();
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTheme();
            SelectedItem = TopNavigationList.FirstOrDefault();
            AppEvents.Instance.ThemeChanged += SwitchTheme;
        }

        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            AppEvents.Instance.ThemeChanged -= SwitchTheme;
        }

        private async void SwitchTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
            }); 
        }


        private void OnShrinkClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            BottomListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            BottomListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
            ShrinkButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
        }

        private void OnExpandClicked(object sender, RoutedEventArgs e)
        {

            // Call GoToState to switch to "State2"
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            BottomListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            BottomListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
            ExpandButton.Visibility = Visibility.Collapsed;
            ShrinkButton.Visibility = Visibility.Visible;
        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Navigation selectedItem = TopListView.SelectedItem as Navigation;

            if (selectedItem.Text == "Transactions")
            {
                ContentFrame.Navigate(typeof(TransactionsPage));
            }
            else if (selectedItem.Text == "Accounts")
            {
                ContentFrame.Navigate(typeof(AccountsPage));
            }
            else
            {
                ContentFrame.Navigate(typeof(DashboardPage));
            }
        }

        private async void SettingsWindowOpen()
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
            CoreDispatcherPriority.Normal,
            async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();

                newAppView.Title = "Settings";
                var frame = new Frame();
                frame.Navigate(typeof(SettingsPage));
                newWindow.Content = frame;

                newWindow.Activate();
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                            newAppView.Id,
                            ViewSizePreference.UseMinimum,
                            currentAV.Id,
                            ViewSizePreference.UseMinimum);
            });
        }

        private void TopListView_Loaded(object sender, RoutedEventArgs e)
        {
            TopListView.SelectedIndex = 0;
        }

        private void BottomListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Navigation selectedItem = e.ClickedItem as Navigation;
            if (selectedItem.Text == "Settings")
            {
                SettingsWindowOpen();
            }
            else if (selectedItem.Text == "Switch Theme")
            {
                if (this.RequestedTheme == ElementTheme.Dark)
                {
                    AppEvents.Instance.OnThemeChanged(ElementTheme.Light);
                }
                else
                {
                    AppEvents.Instance.OnThemeChanged(ElementTheme.Dark);
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }

    public class Navigation
    {
        public Navigation(string text, string iconSource, bool isToggled = false)
        {
            Text = text;
            IconSource = iconSource;
            IsToggled = isToggled;
        }

        public string Text { get; set; }

        public bool IsToggled { get; set; }

        public string IconSource { get; set; }

        public Type PageType { get; set; }
    }

}
