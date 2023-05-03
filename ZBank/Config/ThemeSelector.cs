using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ZBank.Config
{
    public static class ThemeSelector
    {
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

        private static async Task SetThemeAllWindows()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                    }
                });
            }
        }
    }
}
