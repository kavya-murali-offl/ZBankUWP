using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.DataStore;
using ZBank.Services;
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EntryPage : Page, IView
    {
        public EntryPageViewModel ViewModel { get; set; }   

        public EntryPage()
        {
            this.InitializeComponent();
            ViewModel = new EntryPageViewModel(this);
            InitializeThemeSettings();
        }
        private void InitializeThemeSettings()
        {
            ThemeSelector.SetRequestedTheme(RootGrid, Window.Current.Content, ApplicationView.GetForCurrentView().TitleBar);
            ViewNotifier.Instance.ThemeChanged += ThemeSelector_OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged += ThemeSelector_OnAccentColorChanged;
        }

        private async void ThemeSelector_OnAccentColorChanged(Color color)
        {
            await Dispatcher.CallOnUIThreadAsync(()=> {
                ThemeSelector.SetRequestedAccentColor();
                ThemeSelector.ApplyThemeForTitleBarButtons(ApplicationView.GetForCurrentView().TitleBar, ThemeSelector.Theme);
            });
        }

        private async void ThemeSelector_OnThemeChanged(ElementTheme theme)
        {
            await Dispatcher.CallOnUIThreadAsync(() =>
            {
                ThemeSelector.SetRequestedTheme(RootGrid, Window.Current.Content, ApplicationView.GetForCurrentView().TitleBar);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var customerID = AppSettings.Current.CustomerID;
            Repository.Current.CurrentUserID = customerID;
            UpdateFrame(customerID);
        }

        private void UpdateFrame(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                MainFrame.Navigate(typeof(LoginPage));
            }
            else
            {
                MainFrame.Navigate(typeof(MainPage), new MainPageArgs()
                {
                    CustomerID = id
                });
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CurrentUserChanged += CurrentUserChanged;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.CurrentUserChanged -= CurrentUserChanged;

        }

        private void CurrentUserChanged(string id)
        {
            AppSettings.Current.CustomerID = id;
            Repository.Current.CurrentUserID = id;
            UpdateFrame(id);
        }
    }
}
