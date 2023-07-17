using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Entities.BusinessObjects;
using ZBank.Services;
using ZBankManagement.AppEvents;
using ZBankManagement.AppEvents.AppEventArgs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZBank.View.UserControls
{
    public sealed partial class AppNotification : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private List<Notification> NotificationStack { get; set; }

        private Notification _onViewNotification { get; set; }

        public Notification OnViewNotification
        {
            get { return _onViewNotification; }
            set
            {
                _onViewNotification = value;
                OnPropertyChanged(nameof(OnViewNotification));
            }
        }

        private DispatcherQueueTimer Timer { get; set; }

        public AppNotification()
        {
            this.InitializeComponent();
        }

        private void UpdateStack(Notification notification)
        {
            if (NotificationStack.Count == 0)
            {
                NotificationStack.Add(notification);
                Show(NotificationStack[0]);
            }
            else
            {
                NotificationStack.Add(notification);
            }
        }
       

        private void CloseNotification(DispatcherQueueTimer sender, object args)
        {
            CloseAndShowNext();
        }

        private void CloseAndShowNext()
        {
            Timer.Stop();
            NotificationPanel.Visibility = Visibility.Collapsed;
            NotificationStack.RemoveAt(0);
            if (NotificationStack.Count > 0)
            {
                Show(NotificationStack.First());
            }
        }

        private void Show(Notification notification)
        {
                NotificationPanel.Visibility = Visibility.Visible;
                NotificationPanel.DataContext = notification;
                if (notification.Duration > 0)
                {
                    OnViewNotification = notification;
                    Timer.Interval = TimeSpan.FromMilliseconds(notification.Duration);
                    Timer.Start();
                }
                else
                {
                    Timer.Stop();
                }
           
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAndShowNext();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.NotificationStackUpdated += UpdateStack;
            DispatcherQueue queue = DispatcherQueue.GetForCurrentThread();
            Timer = queue.CreateTimer();
            Timer.Tick += CloseNotification;
            NotificationStack = new List<Notification>();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewNotifier.Instance.NotificationStackUpdated -= UpdateStack;
            Timer.Tick -= CloseNotification;
            NotificationStack.Clear();
        }
    }
}
