using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Services;

namespace ZBank.ViewModel
{
    public class EntryPageViewModel
    {
        internal void OnNavigatedTo()
        {
            EnterApplication();
        }

        private void EnterApplication()
        {
            string customerID = AppSettings.Current.CustomerID;
            if (!string.IsNullOrEmpty(customerID))
            {
                FrameContentChangedArgs args = new FrameContentChangedArgs()
                {
                    PageType = typeof(MainPage),
                    Params = new MainPageArgs()
                    {
                        CustomerID = customerID
                    }
                };
                ViewNotifier.Instance.OnFrameContentChanged(args);
            }
            else
            {

            }
        }
    }
}
