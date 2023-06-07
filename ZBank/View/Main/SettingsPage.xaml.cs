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
using ZBank.AppEvents;
using ZBank.Config;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = ThemeSelector.Theme;

        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged += ChangeTheme;
        }

        public void Page_UnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= ChangeTheme;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ElementTheme updatedTheme;

            if (this.RequestedTheme == ElementTheme.Light)
            {
                updatedTheme = ElementTheme.Dark;
            }
            else
            {
                updatedTheme = ElementTheme.Light;
            }
            ViewNotifier.Instance.OnThemeChanged(updatedTheme);
            RequestedTheme = updatedTheme;
        }

        private async void ChangeTheme(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
            });
        }
    }
}
