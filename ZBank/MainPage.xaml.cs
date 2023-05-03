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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZBank
{

    public sealed partial class MainPage : Page
    {

        public IList<Navigation> TopNavigationList { get; private set; }
        public ICommand SwitchThemeCommand;

        public Navigation SelectedItem;

        public MainViewModel ViewModel;

        CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

        public MainPage()
        {
            this.InitializeComponent();
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
            LoadTheme();
            LoadData();
        }

        private void LoadTitleBar()
        {
            coreTitleBar.ExtendViewIntoTitleBar = true;

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            if(ThemeSelector.Theme == ElementTheme.Light)
            {
                titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];
            }
            else
            {
                titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemAltHighColor"];
                titleBar.ForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"];
            }

            titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemAccentColorDark3"];
            titleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["SystemAccentColorLight1"];

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            Window.Current.CoreWindow.Activated += CoreWindow_Activated;

        }

        private void LoadTheme()
        {
            ThemeSelector.InitializeTheme();
            LoadTitleBar();
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
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedItem = TopNavigationList.FirstOrDefault();
        }

        private async void SwitchTheme(object parameter)
        {
                if (ThemeSelector.Theme == ElementTheme.Light)
                {
                    await ThemeSelector.SetTheme(ElementTheme.Dark);
                }
                else if (ThemeSelector.Theme == ElementTheme.Dark)
                {
                    await ThemeSelector.SetTheme(ElementTheme.Light);
                }
                else
                {
                    await ThemeSelector.SetTheme(ElementTheme.Default);
                }
            LoadTitleBar();
        }



        private void OnShrinkClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
            ShrinkButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
            IconContainer.Orientation = Orientation.Vertical;
        }

        private void OnExpandClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
            ExpandButton.Visibility = Visibility.Collapsed;
            ShrinkButton.Visibility = Visibility.Visible;
            IconContainer.Orientation = Orientation.Horizontal;
        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           Navigation selectedItem = TopListView.SelectedItem as Navigation;
            if(selectedItem.Text == "Transactions")
            {
              ContentFrame.Navigate(typeof(TransactionsPage));
            }
            else
            {
                ContentFrame.Navigate(typeof(DashboardPage));
            }
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void CoreWindow_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                //AppTitleTextBlock.Foreground =
                //(Color)(Application.Current.Resources["SystemBaseHighColor"]);
            }
            else
            {
                //AppTitleTextBlock.Foreground =
                //   (SolidColorBrush)(Application.Current.Resources["SystemBaseLowColor"]);
            }
        }

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
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
                frame.RequestedTheme = ThemeSelector.Theme;
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

        private void ThemeToggleButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (ThemeSelector.Theme == ElementTheme.Dark)
                ThemeToggleButton.IsChecked = true;
            else
                ThemeToggleButton.IsChecked = false;
        }

        private void TopListView_Loaded(object sender, RoutedEventArgs e)
        {
            TopListView.SelectedIndex = 0;
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
