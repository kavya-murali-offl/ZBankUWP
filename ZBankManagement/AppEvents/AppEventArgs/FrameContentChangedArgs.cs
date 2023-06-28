using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.AppEvents.AppEventArgs
{
    public class FrameContentChangedArgs
    {
        public Type PageType { get; set; }

        public object Params { get; set; }

    }
}
