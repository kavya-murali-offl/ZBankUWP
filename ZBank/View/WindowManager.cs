using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.Services;
using ZBank.View.Modals;

namespace ZBank.View
{
    public class WindowManager
    {


        public async Task OpenNewWindow<T>(string title, object dataContext=null) where T : Page
        {
            await WindowManagerService.Current.TryShowAsStandaloneAsync(title, typeof(T));
            //var currentAV = ApplicationView.GetForCurrentView();
            //var coreAppView = CoreApplication.CreateNewView();
            //CoreApplication.GetCurrentView().Properties["SecondaryWindowId"] = coreAppView;
            //AllWindows.Add(coreAppView., coreAppView);
            //await coreAppView.Dispatcher.RunAsync(
            //CoreDispatcherPriority.Normal,
            //async () =>
            //{
            //    var newWindow = Window.Current;
            //    var newCurrentAppView = ApplicationView.GetForCurrentView();
            //    newCurrentAppView.Title = title;
            //    var frame = new Frame();
            //    frame.RequestedTheme = ThemeSelector.Theme;
            //    frame.Navigate(typeof(T), dataContext);
            //    newWindow.Content = frame;
            //    newWindow.Activate();
            //    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
            //                newCurrentAppView.Id,
            //                ViewSizePreference.UseMore,
            //                currentAV.Id,
            //                ViewSizePreference.UseMore);
            //});
        }

     

     

    }
}
