﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ZBank.View
{
    public interface IView
    {
        CoreDispatcher Dispatcher { get; }
    }
}
