using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Gaming.Input;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ZBank.AppEvents;
using ZBank.Config;
using ZBank.View.UserControls;
using ZBank.ViewModel;

namespace ZBank.Services
{
    internal class WindowService
    {

        private static Dictionary<int, string> SecondaryViews { get; set; } = new Dictionary<int, string>();
        public static async Task ShowAsync<T>(bool isFullScreenRequested = false)
        {
            int viewId  = -1;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                var frame = new Frame();
                frame.Loaded += FrameLoaded;
                frame.NavigationFailed += (s, e) => { System.Diagnostics.Debugger.Break(); };
                Window.Current.Content = frame;
                Window.Current.Activate();
                frame.Navigate(typeof(T));
                Window.Current.SetTitleBar(new CustomTitleBar());
                var view = ApplicationView.GetForCurrentView();
                if(isFullScreenRequested)
                {
                    view.TryEnterFullScreenMode();
                }
                else
                {
                    if (view.IsFullScreenMode)
                    {
                        view.ExitFullScreenMode();
                    }
                }
                
                view.Consolidated += Helper_Consolidated;
                viewId  = view.Id;  
                SecondaryViews.TryAdd(view.Id, typeof(T).Name);
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId, ViewSizePreference.UseHalf);
            
        }

        private static void FrameLoaded(object sender, RoutedEventArgs e)
        {
            var frame = sender as Frame;
            frame.RequestedTheme = ThemeService.Theme;
        }

        public static async Task ShowOrSwitchAsync<T>(bool isFullScreenRequested = false)
        {
            if(SecondaryViews.ContainsValue(typeof(T).Name)){
                var viewID = SecondaryViews.First(view => view.Value == typeof(T).Name).Key;
                await ApplicationViewSwitcher.SwitchAsync(viewID);
            }
            else
            {
               await ShowAsync<T>(isFullScreenRequested);
            }
        }

        public static void Helper_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            SecondaryViews.Remove(ApplicationView.GetForCurrentView().Id);
            ApplicationView.GetForCurrentView().Consolidated -= Helper_Consolidated;
        }

        public static void CloseWindow(CoreApplicationView view)
        {

                _ = CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    if (CoreApplication.Views.Count == 1)
                        Application.Current.Exit();
                    else if (CoreApplication.Views.Count == 2 && !CoreApplication.MainView.CoreWindow.Visible)
                        Application.Current.Exit();
                    else
                    {
                        _ = view.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            Window.Current.Close();
                        });
                    }
                });
        }

    }
}
