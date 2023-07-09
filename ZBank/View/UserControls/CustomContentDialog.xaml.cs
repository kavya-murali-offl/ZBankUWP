using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using ZBank.View.Modals;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class CustomContentDialog : UserControl
    {

        public CustomContentDialog()
        {
            this.InitializeComponent();
            Dialog = new ContentDialog();
            Dialog.RequestedTheme = ThemeService.Theme;
            Dialog.XamlRoot = this.XamlRoot;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public UserControl DialogContent
        {
            get { return (UserControl)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(UserControl), typeof(CustomContentDialog), new PropertyMetadata(null));

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged += OnThemeChanged;
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.ThemeChanged -= OnThemeChanged;
        }

        private async void OnThemeChanged(ElementTheme theme)
        {
            ThemeService.SetTheme(theme);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Dialog.RequestedTheme = this.RequestedTheme = theme;
            });
        }

        internal async Task OpenDialog()
        {
            if(DialogContent != null) { 
                Dialog.Content = DialogContent;
                await Dialog.ShowAsync();
            }
        }

        public ContentDialog Dialog { get; private set; }

    }
}
