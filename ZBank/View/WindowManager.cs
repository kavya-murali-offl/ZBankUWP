using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        private readonly static Dictionary<int, Window> AllWindows = new Dictionary<int, Window>();

        public async void OpenNewWindow<T>(string title, object dataContext=null) where T : Page
        {

            var currentAV = ApplicationView.GetForCurrentView();
            var coreAppView = CoreApplication.CreateNewView();

            await coreAppView.Dispatcher.RunAsync(
            CoreDispatcherPriority.Normal,
            async () =>
            {
                var newWindow = Window.Current;
                var newCurrentAppView = ApplicationView.GetForCurrentView();
                newCurrentAppView.Title = title;
                var frame = new Frame();
                frame.RequestedTheme = ThemeSelector.Theme;
                frame.Navigate(typeof(T), dataContext);
                newWindow.Content = frame;
                AllWindows.Add(newCurrentAppView.Id, newWindow);
                newWindow.Activate();
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                            newCurrentAppView.Id,
                            ViewSizePreference.UseMore,
                            currentAV.Id,
                            ViewSizePreference.UseMore);
            });
        }

        private void WindowClosed(object sender, CoreWindowEventArgs e)
        {
            int appViewId = ApplicationView.GetForCurrentView().Id;

            if (AllWindows.TryGetValue(appViewId, out var releasingWindow))
            {
                UnSubscribe(appViewId);
                AllWindows.Remove(appViewId);
            };
        }

        private void WindowActivated(object sender, WindowActivatedEventArgs e)
        {
            int appViewId = ApplicationView.GetForCurrentView().Id;

            if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                    UnSubscribe(appViewId);
            }
            else if (e.WindowActivationState == CoreWindowActivationState.CodeActivated ||
                     e.WindowActivationState == CoreWindowActivationState.PointerActivated)
            {
                    Subscribe(appViewId);
            }
        }

        public void Subscribe(int viewId)
        {
            if (AllWindows.ContainsKey(viewId))
            {
                Window window = AllWindows[viewId];
                window.Activated += WindowActivated;
                window.Closed += WindowClosed;
            }
        }


        public void UnSubscribe(int viewId)
        {
            if (AllWindows.ContainsKey(viewId))
            {
                Window window = AllWindows[viewId];
                window.Activated -= WindowActivated;
                window.Closed -= WindowClosed;
            }
        }



        private async void ThemeChangedForNewWindow(ElementTheme theme)
        {
            ThemeSelector.SwitchTheme(theme);
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    ((FrameworkElement)Window.Current.Content).RequestedTheme = this.RequestedTheme = theme;
            //});
        }


    }
}
