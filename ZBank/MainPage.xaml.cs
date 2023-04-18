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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZBank
{

    public sealed partial class MainPage : Page
    {

        public IList<Navigation> TopNavigationList { get; private set; }
        public IList<Navigation> BottomNavigationList { get; private set; }

        public string SelectedItem;

        CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        public MainPage()
        {
            this.InitializeComponent();
            LoadData();

            FrameworkElement root = (FrameworkElement)Window.Current.Content;
            root.RequestedTheme = AppSettings.Theme;
            SetThemeToggle(AppSettings.Theme);

            
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // Set caption buttons background to transparent.
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            // Set XAML element as a drag region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            // Register a handler for when the window activation changes.
            Window.Current.CoreWindow.Activated += CoreWindow_Activated;
        }



        private void SetThemeToggle(ElementTheme theme)
        {
            //if (theme == AppSettings.DEFAULTTHEME)
            //    ThemeToggleButton.IsChecked = false;
            //else
            //    ThemeToggleButton.IsChecked = true;
        }

        public void LoadData()
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
                new Navigation("Theme", "\uE793", true),
                new Navigation("Settings", "\uE713"),
            };
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //TopAppMenuList.SelectedItem = NavigationList.FirstOrDefault();
        }

      
        private void OnShrinkClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            TopListView.ItemContainerStyle = NarrowMenuListItemStyle;
            ShrinkButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
        }

        private void OnExpandClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            TopListView.ItemContainerStyle = WideMenuListItemStyle;
            ExpandButton.Visibility = Visibility.Collapsed;
            ShrinkButton.Visibility = Visibility.Visible;
        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Console.WriteLine(BottomListView.SelectedItem.ToString());
            ContentFrame.Navigate(typeof(Dashboard));
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement window = (FrameworkElement)Window.Current.Content;
            if (AppSettings.Theme == AppSettings.DEFAULTTHEME)
            {
                AppSettings.Theme = AppSettings.NONDEFLTHEME;
                window.RequestedTheme = AppSettings.NONDEFLTHEME;
            }
            else
            {
                AppSettings.Theme = AppSettings.DEFAULTTHEME;
                window.RequestedTheme = AppSettings.DEFAULTTHEME;
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            // Get the size of the caption controls and set padding.
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
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
            UISettings settings = new UISettings();
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                   new SolidColorBrush(settings.UIElementColor(UIElementType.GrayText));
            }
            else
            {
                AppTitleTextBlock.Foreground =
                   new SolidColorBrush(settings.UIElementColor(UIElementType.WindowText));
            }
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
