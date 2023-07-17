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
using ZBank.ViewModel.VMObjects;

namespace ZBank.Services
{
    internal class DialogService
    {
        public static async Task ShowActionDialogAsync(string content, Action<object> callback, string title = null, string okButtonText = "Yes", string cancelButtonText = "Cancel")
        {
            var dialog = new ContentDialog()
            {
                Content = content,
                XamlRoot = Window.Current.Content.XamlRoot,
                Title = title,
                RequestedTheme = ThemeService.Theme,
                PrimaryButtonCommand = new RelayCommand(callback),
                PrimaryButtonText = "Yes",
            };
            dialog.SecondaryButtonCommand = new RelayCommand(CloseDialog);
            dialog.SecondaryButtonCommandParameter = dialog;
            dialog.SecondaryButtonText = "Cancel";

            await DispatcherService.CallOnMainViewUiThreadAsync(async () =>
                await dialog.ShowAsync()
            );
        }

        private static async void CloseDialog(object dialog)
        {
            if(dialog is ContentDialog contentDialog) {
                await DispatcherService.CallOnMainViewUiThreadAsync(() =>
                {
                    contentDialog.Hide();
                });
            }
        }


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
                ViewNotifier.Instance.CloseDialog += () =>
                {
                    contentDialog.Hide();
                };
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
