using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    public class Notification
    {
        public Notification(string title, string description) {
            Title = title;
            Description = description;
        }

        public string ID { get; set; }  

        public string Title { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }
    }
}
