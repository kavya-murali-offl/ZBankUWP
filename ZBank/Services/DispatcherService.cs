using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace ZBank.Services
{
    public static class DispatcherService
    {
        public static async Task CallOnUIThreadAsync(this CoreDispatcher dispatcher, DispatchedHandler handler)
        {
            if (dispatcher.HasThreadAccess)
            {
                handler.Invoke();
            }
            else
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, handler);
            }
        }

        public static async Task CallOnMainViewUiThreadAsync(DispatchedHandler handler) =>
            await CoreApplication.MainView.CoreWindow.Dispatcher.CallOnUIThreadAsync(handler);
    }
}
