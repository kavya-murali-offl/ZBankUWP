using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.Config;
using ZBank.View.Modals;

namespace ZBank.View
{
    public class WindowManager
    {

        ConcurrentDictionary<int, EventHandler> windowEvents = new ConcurrentDictionary<int, EventHandler>(); 

        public async void OpenNewWindow<T>(string title, object dataContext) where T : Page
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
            CoreDispatcherPriority.Normal,
            async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;
                var frame = new Frame();
                frame.RequestedTheme = ThemeSelector.Theme;
                frame.Navigate(typeof(T), dataContext);
                newWindow.Content = frame;
                windowEvents[newAppView.Id] = WindowOpened;
                newWindow.Activate();
                newWindow.Closed += WindowClosed;
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                            newAppView.Id,
                            ViewSizePreference.UseMinimum,
                            currentAV.Id,
                            ViewSizePreference.UseMinimum);
            });
        }

        private void WindowClosed(object sender, CoreWindowEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WindowOpened(object sender, EventArgs e)
        {

        }

        private void EventsSubscribe(object sender, WindowActivatedEventArgs e)
        {
            
        }
    }
}
