using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.AppEvents.AppEventArgs;
using ZBank.AppEvents;
using ZBank.Entities;
using ZBank.Services;
using ZBank.Config;
using ZBank.View;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;

namespace ZBank.ViewModel
{
    public class EntryPageViewModel
    {
        private IView View { get; set; }

        public EntryPageViewModel(IView view) 
        { 
            View = view; 
            ThemeSelector.Initialize();
        }

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
