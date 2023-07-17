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
using Windows.UI.Xaml.Controls.Primitives;
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

        private IEnumerable<FrameworkElement> Contents { get; set; } = new List<FrameworkElement>();    

        public static Task ShowContentAsync(IView view, FrameworkElement content, string title, XamlRoot xamlRoot = null)
        {
            return view.Dispatcher.CallOnUIThreadAsync(async() =>
            {
                var contentDialog = new ContentDialog()
                {
                    Title = title,
                    Content = content,
                    XamlRoot = xamlRoot,
                    RequestedTheme = ThemeService.Theme,
                    MinWidth = 500
                };
                contentDialog.MinWidth = 400;
                ViewNotifier.Instance.CloseDialog += () => contentDialog.Hide();
                ViewNotifier.Instance.ThemeChanged += async (ElementTheme theme) =>
                {
                    if (view.Dispatcher.HasThreadAccess)
                    {
                        content.RequestedTheme = theme;
                    }
                    else
                    {
                        await view.Dispatcher.CallOnUIThreadAsync(() =>
                      {
                          ((FrameworkElement)xamlRoot.Content).RequestedTheme = theme;
                          contentDialog.RequestedTheme = theme;
                      });
                    }
                };
                
                await contentDialog.ShowAsync();
            });
        }
    }
}
