﻿using Windows.ApplicationModel.Core;
using Windows.UI.Core;
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

        private void LoadTitleBar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.CoreWindow.Activated += CoreWindow_Activated;

            Window.Current.SetTitleBar(AppTitleBar);
            ThemeSelector.UpdateTitleBarTheme();
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

        }

        private void CoreWindow_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                (SolidColorBrush)(Application.Current.Resources["ApplicationBackgroundThemeBrush"]);
            }
            else
            {
                AppTitleTextBlock.Foreground =
                   (SolidColorBrush)(Application.Current.Resources["ApplicationForeground"]);
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
    }
}
