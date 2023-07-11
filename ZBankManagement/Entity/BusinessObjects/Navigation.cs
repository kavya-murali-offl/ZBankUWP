using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.Entities.BusinessObjects
{
    public class Navigation
    {
        public Navigation(string text, string iconSource, params Type[] pageTypes)
        {
            Text = text;
            Tag = text;
            IconSource = iconSource;
            PageTypes = pageTypes;
        }

        public string Text { get; set; }

        public string Tag { get; set; }

        public string IconSource { get; set; }

        public Type[] PageTypes { get; set; }
    }
}
