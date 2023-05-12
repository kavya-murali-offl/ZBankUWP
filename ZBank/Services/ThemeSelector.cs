using ZBankManagement.Events;
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


        public static ElementTheme Theme { get; set; } = ElementTheme.Dark;

        public static void InitializeTheme()
        {
            LoadCacheTheme();
        }

        public static void SwitchTheme(ElementTheme theme)
        {
            LocalSettings.Values[key] = theme.ToString();
            Theme = theme;
        }

        public static string GetIcon()
        {
            return Theme == ElementTheme.Dark ? DarkThemeIcon : LightThemeIcon;
        }

        private static void LoadCacheTheme()
        {
            ElementTheme localTheme = ElementTheme.Dark;

            string themeName = (string)LocalSettings.Values[key];

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out localTheme);
            }
            Theme = localTheme;
        }
    }
}
