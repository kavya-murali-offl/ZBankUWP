using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using ZBank.AppEvents;
using ZBankManagement.AppEvents.AppEventArgs;

namespace ZBankManagement.AppEvents
{

    public class Notification
    {
        public object Content { get; set; }

        public double Duration { get; set; }
        
        public NotificationType Type { get; set; }


    }

    public enum NotificationType { ERROR, INFO, WARNING }
}
