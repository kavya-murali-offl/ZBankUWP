using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.View;

namespace ZBank.Services
{
    internal class DialogService
    {

        public static Task ShowContentAsync(IView view, FrameworkElement content, string title, XamlRoot xamlRoot = null)
        {
            xamlRoot = Window.Current.Content.XamlRoot;
            return view.Dispatcher.CallOnUIThreadAsync(async() =>
            {
                var contentDialog = new ContentDialog()
                {
                    Title = title,
                    Content = content,
                    XamlRoot = xamlRoot,
                    RequestedTheme = ThemeService.Theme
                };
                ViewNotifier.Instance.CloseDialog += () => contentDialog.Hide();
                await contentDialog.ShowAsync();
            });
        }
    }
}
