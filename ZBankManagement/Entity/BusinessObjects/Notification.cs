using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    public class Notification
    {

        public object Message { get; set; }

        public double Duration { get; set; } = 3000;

        public NotificationType Type { get; set; }
    }


    public enum NotificationType { ERROR, INFO, WARNING, SUCCESS }
}
