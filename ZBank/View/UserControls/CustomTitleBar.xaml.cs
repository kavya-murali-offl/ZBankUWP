using ZBankManagement.Events;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ZBank.Config;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZBank.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomTitleBar : Page
    {
        public CustomTitleBar()
        {
            this.InitializeComponent();
            LoadTitleBar();

        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CustomTitleBar), new PropertyMetadata(null));


        private void LoadTitleBar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.CoreWindow.Activated += CoreWindow_Activated;

            Window.Current.SetTitleBar(AppTitleBar);
            UpdateTitleBarTheme(ThemeSelector.Theme);
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreWindow_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                (SolidColorBrush)(Application.Current.Resources["ApplicationForeground"]);
                AppTitleBar.Background =
                (SolidColorBrush)(Application.Current.Resources["ApplicationBackgroundThemeBrush"]);
            }
            else
            {
                AppTitleTextBlock.Foreground =
                   (SolidColorBrush)(Application.Current.Resources["ApplicationForeground"]);
                AppTitleBar.Background =
               (SolidColorBrush)(Application.Current.Resources["ApplicationBackground"]);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AppEvents.Instance.ThemeChanged += UpdateTitleBarTheme;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AppEvents.Instance.ThemeChanged -= UpdateTitleBarTheme;
        }

        private void UpdateTitleBarTheme(ElementTheme Theme)
        {
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

                if (Theme == ElementTheme.Light)
                {
                    titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                    titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];
                }
                else
                {
                    titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemAltHighColor"];
                    titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"];
                }

                titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemAccentColorDark3"];
                titleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["SystemAccentColorLight1"];
            
        }
    }
}
