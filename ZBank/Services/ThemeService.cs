using Microsoft.Toolkit.Uwp.Helpers;
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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ZBank.AppEvents;
using ZBank.DataStore;

namespace ZBank.Config
{
    public static class ThemeService
    {
        public static readonly string DarkThemeIcon = "\uEC46";
        public static readonly string LightThemeIcon = "\uE793";
        private const string themeKey = "Theme";
        public static ElementTheme Theme { get; set; } = ElementTheme.Default;

        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        private static readonly UISettings UISettings = new UISettings();

        private static readonly string LightBackgroundColor = "#FFFFFFFF";
        private static readonly string DarkBackgroundColor = "#FF000000";

        public static void Initialize()
        {
            InitializeTheme();
            InitializeAppAccentColor();
            InitializeCustomAccentColor();
        }

        public static void InitializeTheme()
        {
            LoadCacheTheme();
        }

        private static Color _customAccentColor;

        public static Color CustomAccentColor
        {
            get => _customAccentColor;
            set
            {
                _customAccentColor = value;
                LocalSettings.Values["CustomAccentColorHexStr"] = value.ToHex();
            }
        }

        private static Color _appAccentColor;
        public static Color AppAccentColor
        {
            get => _appAccentColor;
            set
            {
                _appAccentColor = value;
                ViewNotifier.Instance.OnAccentColorChanged(_appAccentColor);
                LocalSettings.Values["AppAccentColorHexStr"] = value.ToHex();
            }
        }

        public static void SetRequestedTheme(UIElement currentContent, ApplicationViewTitleBar titleBar, ElementTheme theme)
        {

            if (currentContent is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = Theme;
            }

            SetTheme(theme);

            ((SolidColorBrush)Application.Current.Resources["SystemControlPageBackgroundMediumAltMediumBrush"]).Color =
            Theme == ElementTheme.Dark ? Color.FromArgb(153, 0, 0, 0) : Color.FromArgb(153, 255, 255, 255);


            UpdateSystemAccentColorAndBrushes(AppAccentColor);

            if (titleBar != null)
            {
                ApplyThemeForTitleBarButtons(titleBar, Theme);
            }
        }

        private static bool _useWindowsAccentColor;

        public static bool UseWindowsAccentColor
        {
            get => _useWindowsAccentColor;
            set
            {
                _useWindowsAccentColor = value;
                if (value)
                {
                    AppAccentColor = UISettings.GetColorValue(UIColorType.Accent);
                }
                LocalSettings.Values["UseWindowsAccentColorBool"] = _useWindowsAccentColor;
            }
        }

        private static void InitializeCustomAccentColor()
        {
            if (LocalSettings.Values["CustomAccentColorHexStr"] is string customAccentColorHexStr)
            {
                _customAccentColor = customAccentColorHexStr.ToColor();
            }
            else
            {
                _customAccentColor = _appAccentColor;
            }
        }
        private static void InitializeAppAccentColor()
        {
            if (LocalSettings.Values["UseWindowsAccentColorBool"] is bool useWindowsAccentColor)
            {
                _useWindowsAccentColor = useWindowsAccentColor;
            }
            else
            {
                _useWindowsAccentColor = true;
            }

            UISettings.ColorValuesChanged += UiSettings_ColorValuesChanged;

            _appAccentColor = UISettings.GetColorValue(UIColorType.Accent);

            if (!UseWindowsAccentColor)
            {
                if (LocalSettings.Values["AppAccentColorHexStr"] is string accentColorHexStr)
                {
                    _appAccentColor = accentColorHexStr.ToColor();
                }
            }
        }

        private static bool IsDarkTheme()
        {
            string backgroundColor = UISettings.GetColorValue(UIColorType.Background).ToString();
            if(Theme == ElementTheme.Default)
            {
                return  backgroundColor == DarkBackgroundColor;
            }
            return Theme == ElementTheme.Dark;
        }

        private static void UiSettings_ColorValuesChanged(UISettings sender, object args)
        {
            if (UseWindowsAccentColor)
            {
                AppAccentColor = sender.GetColorValue(UIColorType.Accent);
            }
        }

        public static void ApplyThemeForTitleBarButtons(ApplicationViewTitleBar titleBar, ElementTheme theme)
        {
            if (IsDarkTheme())
            {
                titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ButtonForegroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];
                titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ButtonPressedForegroundColor = (Color)Application.Current.Resources["SystemBaseLowColor"];
                titleBar.ButtonPressedBackgroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"];
                titleBar.ButtonInactiveForegroundColor = (Color)Application.Current.Resources["SystemAltLowColor"];
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                titleBar.InactiveForegroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"]; ;
                titleBar.InactiveBackgroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];
            }
            else
            {
                titleBar.ButtonBackgroundColor = (Color)Application.Current.Resources["SystemAltHighColor"];
                titleBar.ButtonForegroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"];
                titleBar.ButtonHoverForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                titleBar.ButtonPressedForegroundColor = (Color)Application.Current.Resources["SystemBaseLowColor"];
                titleBar.ButtonPressedBackgroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];
                titleBar.ButtonInactiveForegroundColor = (Color)Application.Current.Resources["SystemBaseLowColor"];
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                titleBar.InactiveForegroundColor = (Color)Application.Current.Resources["SystemBaseMediumColor"]; ;
                titleBar.InactiveBackgroundColor = (Color)Application.Current.Resources["SystemAltMediumColor"];

            }

            titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)Application.Current.Resources["AccentColorBrush"]).Color;
        }

        private static void UpdateSystemAccentColorAndBrushes(Color color)
        {
            SolidColorBrush customAccentBrush = (SolidColorBrush)Application.Current.Resources["AccentColorBrush"];
            customAccentBrush.Color = color;
            var brushes = new string[]
            {
                "SystemControlBackgroundAccentBrush",
                "SystemControlDisabledAccentBrush",
                "SystemControlForegroundAccentBrush",
                "SystemControlHighlightAccentBrush",
                "SystemControlHighlightAltAccentBrush",
                "SystemControlHighlightAltListAccentHighBrush",
                "SystemControlHighlightAltListAccentLowBrush",
                "SystemControlHighlightAltListAccentMediumBrush",
                "SystemControlHighlightListAccentHighBrush",
                "SystemControlHighlightListAccentLowBrush",
                "SystemControlHighlightListAccentMediumBrush",
                "SystemControlHyperlinkTextBrush",
                "ContentDialogBorderThemeBrush",
                "JumpListDefaultEnabledBackground",
                "SystemControlHighlightAccent3RevealBackgroundBrush",
                "SystemControlHighlightAccent2RevealBackgroundBrush",

            };

            foreach (var brush in brushes)
            {
                try
                {
                    ((SolidColorBrush)Application.Current.Resources[brush]).Color = color;
                }
                catch { }
            }

            try
            {
                ((RevealBackgroundBrush)Application.Current.Resources["SystemControlHighlightAccent3RevealBackgroundBrush"]).Color = color;
            }
            catch { }
        }

        public static void SetRequestedAccentColor()
        {
            UpdateSystemAccentColorAndBrushes(AppAccentColor);
            ApplyThemeForTitleBarButtons(ApplicationView.GetForCurrentView().TitleBar, Theme);
        }


        public static void SetTheme(ElementTheme theme)
        {
            if (Theme != theme)
            {
                Theme = theme;
                LocalSettings.Values[themeKey] = theme.ToString();
                ViewNotifier.Instance.OnThemeChanged(theme);
            }
        }

        public static string GetIcon()
        {
            return Theme == ElementTheme.Dark ? DarkThemeIcon : LightThemeIcon;
        }

        private static void LoadCacheTheme()
        {
            ElementTheme localTheme = ElementTheme.Default;

            string themeName = (string)LocalSettings.Values[themeKey];

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out localTheme);
            }
            Theme = localTheme;
        }
    }
}
