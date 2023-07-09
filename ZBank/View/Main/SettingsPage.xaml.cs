using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
using ZBank.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page, IView
    {
        private SettingsViewModel ViewModel { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(AppTitleBar);
            ViewModel = new SettingsViewModel(this);
            UseWindowsAccentColor.IsOn = ThemeService.UseWindowsAccentColor;
            if(ThemeService.Theme == ElementTheme.Default) SystemToggleButton.IsChecked = true;    
            else if(ThemeService.Theme == ElementTheme.Light) LightToggleButton.IsChecked = true;    
            else if(ThemeService.Theme == ElementTheme.Dark) DarkToggleButton.IsChecked = true;    
        }

        private async void ThemeSelector_OnAccentColorChanged(Color color)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ThemeService.SetRequestedTheme(null, Window.Current.Content, titleBar);
            });
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged += OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged += ThemeSelector_OnAccentColorChanged;
        }

        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
            ViewNotifier.Instance.AccentColorChanged -= ThemeSelector_OnAccentColorChanged;
        }

        private void AccentButton_Click(object sender, RoutedEventArgs e)
        {
            ThemeService.UseWindowsAccentColor = false;
            SolidColorBrush selectedBrush = ((FrameworkElement)sender).DataContext as SolidColorBrush;
            var color = selectedBrush.Color;
            ThemeService.AppAccentColor = color;
        }

        private async void OnThemeChanged(ElementTheme theme)
        {
            ThemeService.SetTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
            });
        }

        private void UseWindowsAccentColor_Toggled(object sender, RoutedEventArgs e)
        {
            ThemeService.UseWindowsAccentColor = UseWindowsAccentColor.IsEnabled;
        }

        private void SystemToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton clickedButton = (ToggleButton)sender;
            string buttonName = clickedButton.Name;

            ElementTheme updatedTheme = ElementTheme.Default;

            switch (buttonName)
            {
                case "SystemToggleButton":
                    SystemToggleButton.IsChecked = true;
                    DarkToggleButton.IsChecked = false;
                    LightToggleButton.IsChecked = false;
                    updatedTheme = ElementTheme.Default;
                    break;
                case "LightToggleButton":
                    SystemToggleButton.IsChecked = false;
                    DarkToggleButton.IsChecked = false;
                    LightToggleButton.IsChecked = true;
                    updatedTheme = ElementTheme.Light;
                    break;
                case "DarkToggleButton":
                    SystemToggleButton.IsChecked = false;
                    DarkToggleButton.IsChecked = true;
                    LightToggleButton.IsChecked = false;
                    updatedTheme = ElementTheme.Dark;
                    break;
                default:
                    break;
            }

            ViewNotifier.Instance.OnThemeChanged(updatedTheme);
        }
    }
}
