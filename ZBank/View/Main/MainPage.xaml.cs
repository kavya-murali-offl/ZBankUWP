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
using Windows.UI.Xaml.Controls.Primitives;
using System.ServiceModel.Channels;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using ZBankManagement.AppEvents.AppEventArgs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ZBank.Entity.BusinessObjects;
using ZBank.Entities;
using ZBank.View.UserControls;


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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnLoaded();
            LoadToggleButton();
            ViewNotifier.Instance.RightPaneContentUpdated += OnRightPaneContentUpdated;
            ViewNotifier.Instance.ThemeChanged += SwitchTheme;
            ViewNotifier.Instance.FrameContentChanged += ChangeFrame;
            ContentFrame.Navigate(typeof(DashboardPage));
            ContentFrame.Navigated += OnNavigated;
        }

        private FrameworkElement SecondarySplitViewContent { get; set; }

        private void SecondarySplitView_PaneClosed(SplitView sender, object args)
        {
            //if (SecondarySplitViewContent != null)
            //{
            //    SecondarySplitView.IsPaneOpen = true;
            //}

        }

        private void SecondarySplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            //if (SecondarySplitViewContent != null)
            //{
            //    SecondarySplitView.IsPaneOpen = true;
            //}
        }

        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.RightPaneContentUpdated -= OnRightPaneContentUpdated;
            ViewNotifier.Instance.ThemeChanged -= SwitchTheme;
            ViewNotifier.Instance.FrameContentChanged -= ChangeFrame;
            ContentFrame.Navigated -= OnNavigated;
            ViewModel.OnUnloaded();
        }

        private void OnRightPaneContentUpdated(FrameworkElement obj)
        {
            if (obj != null)
            {
                SecondarySplitView.IsPaneOpen = true;
                RightPaneContent.Content = obj;
                SecondarySplitViewContent = obj;
            }
            else
            {
                SecondarySplitView.IsPaneOpen = false;
                RightPaneContent.Content = SecondarySplitViewContent = obj;
            }
            
        }

        public void OnNavigated(object sender, NavigationEventArgs e)
        {
            ViewModel.UpdateSelectedPage(ContentFrame.CurrentSourcePageType);
        }

        

        private void OnShrinkClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
        }

        private void OnExpandClicked(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
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
            ThemeSelector.SetTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
                ThemeIcon.Glyph = ThemeSelector.GetIcon();
                SwitchThemeButton.IsChecked = false;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void TopListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var selectedItem = (sender as ListViewItem).DataContext as Navigation;
            //if(selectedItem != null)
            //{
            //    ViewModel.NavigationChanged(selectedItem);
            //}
        }

        public double initialPaneWidth { get; set; }

        public bool isDragging { get; set; }

        private void MySplitView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isDragging = true;
            MySplitView.CapturePointer(e.Pointer);
        }

        private void MySplitView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (MySplitView.IsPaneOpen && e.GetCurrentPoint(MySplitView.Pane).Position.X >= MySplitView.OpenPaneLength)
            {
                ClosePane();
            }

        }


        private void Content_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (isDragging)
            {
                if (MySplitView.IsPaneOpen && e.GetCurrentPoint(MySplitView).Position.X <= MySplitView.OpenPaneLength)
                {
                    ClosePane();
                }
                else if (e.GetCurrentPoint(MySplitView).Position.X >= MySplitView.CompactPaneLength)
                {
                    OpenPane();
                }
            }
           
            MySplitView.ReleasePointerCapture(e.Pointer);
            isDragging = false;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            ResizeBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void ClosePane()
        {
            MySplitView.IsPaneOpen = false;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["NarrowTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["NarrowMenuListItemStyle"];
        }

        private void OpenPane()
        {
            MySplitView.IsPaneOpen = true;
            TopListView.ItemTemplate = (DataTemplate)this.Resources["WideTopDataTemplate"];
            TopListView.ItemContainerStyle = (Style)Application.Current.Resources["WideMenuListItemStyle"];
        }

        private void ResizeBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
            ResizeBorder.Background = (SolidColorBrush)Application.Current.Resources["BorderBrush"];
        }

        private void ResizeBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ResizeBorder.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

        private void MySplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            ClosePane();
            MySplitView.IsPaneOpen = false;
        }

        private void SignoutButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Signout();
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

