using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.Config;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.Main
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomPage : Page
    {
        public CustomPage()
        {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadTheme();
            LoadTitleBar();
        }

        private void LoadTitleBar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            coreTitleBar.ExtendViewIntoTitleBar = true;

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            if (ThemeSelector.Theme == ElementTheme.Light)
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
    }
}
