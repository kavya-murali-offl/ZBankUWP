using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.Config;
using ZBank.View;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using ZBank.View.Main;
using ZBank.ViewModel;
using ZBank.AppEvents;
using ZBank.AppEvents.AppEventArgs;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Navigation;
using System.Linq;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZBank
{
    public sealed partial class MainPage : Page, IView
    {
        public MainViewModel ViewModel { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel(this);
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTheme();
            LoadToggleButton();
            ViewNotifier.Instance.ThemeChanged += SwitchTheme;
            ViewNotifier.Instance.FrameContentChanged += ChangeFrame;
            ContentFrame.Navigate(typeof(DashboardPage));
            ContentFrame.Navigated += OnNavigated;
        }


        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= SwitchTheme;
            ViewNotifier.Instance.FrameContentChanged -= ChangeFrame;
            ContentFrame.Navigated += OnNavigated;
        }

        public void OnNavigated(object sender, NavigationEventArgs e)
        {
            ViewModel.UpdateSelectedPage(ContentFrame.CurrentSourcePageType);
        }

        private void LoadTheme()
        {
            ThemeSelector.InitializeTheme();
            ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = ThemeSelector.Theme;
        }

        private void OnShrinkClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
            ShrinkButton.Visibility = Visibility.Collapsed;
            ExpandButton.Visibility = Visibility.Visible;
        }

        private void OnExpandClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
            ExpandButton.Visibility = Visibility.Collapsed;
            ShrinkButton.Visibility = Visibility.Visible;
        }

        private void Navigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Navigation selectedItem = TopListView.SelectedItem as Navigation;
            ViewModel.NavigationChanged(selectedItem);
        }

        public void ChangeFrame(FrameContentChangedArgs args)
        {
            ContentFrame.Navigate(args.PageType, args.Params);
        }

        private void TopListView_Loaded(object sender, RoutedEventArgs e)
        {
            TopListView.SelectedIndex = 0;
        }

        private async void SwitchTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
                ThemeIcon.Glyph = ThemeSelector.GetIcon();
            });
        }

        private void SwitchThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeSelector.Theme == ElementTheme.Dark)
            {
                ViewNotifier.Instance.OnThemeChanged(ElementTheme.Light);
            }
            else
            {
                ViewNotifier.Instance.OnThemeChanged(ElementTheme.Dark);
            }
        }

        private void LoadToggleButton()
        {
            if (ThemeSelector.Theme == ElementTheme.Dark)
            {
                SwitchThemeButton.IsChecked = true;
            }
            else
            {
                SwitchThemeButton.IsChecked = false;
            }
            ThemeIcon.Glyph = ThemeSelector.GetIcon();
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


        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoForward)
            {
                ContentFrame.GoForward();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
    }

    public class Navigation
    {
        public Navigation(string text, string iconSource, params Type[] pageTypes)
        {
            Text = text;
            Tag = text;
            IconSource = iconSource;
            PageTypes = pageTypes;
        }

        public string Text { get; set; }

        public string Tag { get; set; }

        public string IconSource { get; set; }

        public Type[] PageTypes { get; set; }
    }
}
