using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
using ZBank.Entities;
using ZBank.View.Main;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageHeader : Page
    {
        public PageHeader()
        {
            this.InitializeComponent();
            LoadToggleButton();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PageHeader), new PropertyMetadata(null));


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

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged += SwitchTheme;
        }

        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= SwitchTheme;
        }

        private async void SwitchTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
            });
            ThemeIcon.Glyph = ThemeSelector.GetIcon();
        }

        private void SwitchThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RequestedTheme == ElementTheme.Dark)
            {
                ViewNotifier.Instance.OnThemeChanged(ElementTheme.Light);
            }
            else
            {
                ViewNotifier.Instance.OnThemeChanged(ElementTheme.Dark);
            }
            LoadToggleButton();
           
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

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
