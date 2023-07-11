using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.View;

namespace ZBank.Services
{
    internal class DialogService
    {
        public static async Task ShowActionDialogAsync(string content, Action callback, string title = null, string okButtonText = "Ok", string cancelButtonText = "Cancel")
        {
            var dialog = title == null ?
                new MessageDialog(content) { CancelCommandIndex = 1 } :
                new MessageDialog(content, title) { CancelCommandIndex = 1 };

            dialog.Commands.Add(new UICommand(okButtonText, command => callback()));
            dialog.Commands.Add(new UICommand(cancelButtonText));
            
            await DispatcherService.CallOnMainViewUiThreadAsync(async () =>
            await dialog.ShowAsync()
            );
        }

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
