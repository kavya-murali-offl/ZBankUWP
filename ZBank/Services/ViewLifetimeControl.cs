﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ZBank.Services
{
    public delegate void ViewReleasedHandler(object sender, EventArgs e);

    // Whenever the main view is about to interact with the secondary view, it should call
    // StartViewInUse on this object. When finished interacting, it should call StopViewInUse.
    public sealed class ViewLifetimeControl
    {
        private readonly object _lockObj = new object();

        // Window for this particular view. Used to register and unregister for events
        private CoreWindow _window;
        private int _refCount = 0;
        private bool _released = false;

        private event ViewReleasedHandler InternalReleased;

        // Necessary to communicate with the window
        public CoreDispatcher Dispatcher { get; private set; }

        // This id is used in all of the ApplicationViewSwitcher and ProjectionManager APIs
        public int Id { get; private set; }

        public string Title { get; set; }

        public async Task CloseAsync()
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _window.Close();
            });
        }

        public event ViewReleasedHandler Released
        {
            add
            {
                bool releasedCopy = false;
                lock (_lockObj)
                {
                    releasedCopy = _released;
                    if (!_released)
                    {
                        InternalReleased += value;
                    }
                }

                if (releasedCopy)
                {
                    throw new InvalidOperationException("This view is being disposed.");
                }
            }

            remove
            {
                lock (_lockObj)
                {
                    InternalReleased -= value;
                }
            }
        }

        private ViewLifetimeControl(CoreWindow newWindow)
        {
            Dispatcher = newWindow.Dispatcher;
            _window = newWindow;
            Id = ApplicationView.GetApplicationViewIdForWindow(_window);
            RegisterForEvents();
        }

        public static ViewLifetimeControl CreateForCurrentView()
        {
            return new ViewLifetimeControl(CoreWindow.GetForCurrentThread());
        }

        // Signals that the view is being interacted with by another view,
        // so it shouldn't be closed even if it becomes "consolidated"
        public int StartViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;

            lock (_lockObj)
            {
                releasedCopy = _released;
                if (!_released)
                {
                    refCountCopy = ++_refCount;
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed.");
            }

            return refCountCopy;
        }

        // Should come after any call to StartViewInUse
        // Signals that the another view has finished interacting with the view tracked by this object
        public int StopViewInUse()
        {
            int refCountCopy = 0;
            bool releasedCopy = false;

            lock (_lockObj)
            {
                releasedCopy = _released;
                if (!_released)
                {
                    refCountCopy = --_refCount;
                    if (refCountCopy == 0)
                    {
                        Dispatcher.RunAsync(CoreDispatcherPriority.Low, FinalizeRelease).AsTask();
                    }
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed.");
            }

            return refCountCopy;
        }

        private void RegisterForEvents()
        {
            Window.Current.Closed += OnClosed;
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
        }

        public event EventHandler Closed;

        private void UnregisterForEvents()
        {
            Window.Current.Closed -= OnClosed;
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }

        private void OnClosed(object sender, CoreWindowEventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs e)
        {
            StopViewInUse();
        }

        private void FinalizeRelease()
        {
            bool justReleased = false;
            lock (_lockObj)
            {
                if (_refCount == 0)
                {
                    justReleased = true;
                    _released = true;
                }
            }

            if (justReleased)
            {
                UnregisterForEvents();
                if (InternalReleased == null)
                {
                    // For more information about using Multiple Views, see https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/features/multiple-views.md
                    throw new InvalidOperationException("All pages opened in a new window must subscribe to the Released Event.");
                }

                InternalReleased.Invoke(this, null);
                Window.Current.Content = null;
                Window.Current.Close();
            }
        }
    }
}
