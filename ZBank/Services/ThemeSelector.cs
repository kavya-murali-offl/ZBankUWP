using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ZBank.Config
{
    public static class ThemeSelector
    {
        public static readonly string DarkThemeIcon = "\uEC46";
        public static readonly string LightThemeIcon = "\uE793";
        private const string key = "Theme";

        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        public static async void InitializeTheme()
        {
            LoadCacheTheme();
            SetThemeInSettings(Theme);
            await SetThemeAllWindows();
        }

        public static async Task SetTheme(ElementTheme theme)
        {
            Theme = theme;
            SetThemeInSettings(Theme);
            await SetThemeAllWindows();
        }

        public static string GetIcon()
        {
            return Theme == ElementTheme.Dark ? DarkThemeIcon : LightThemeIcon;
        }

        private static void LoadCacheTheme()
        {
            ElementTheme localTheme = ElementTheme.Default;

            string themeName = (string)LocalSettings.Values[key];

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out localTheme);
            }
            Theme = localTheme;
        }

        private static void SetThemeInSettings(ElementTheme theme)
        {
            LocalSettings.Values[key] = theme.ToString();
        }

        public static  void UpdateTitleBarTheme()
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

        private static async Task SetThemeAllWindows()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                        UpdateTitleBarTheme();
                    }
                });
            }
        }
    }
}
