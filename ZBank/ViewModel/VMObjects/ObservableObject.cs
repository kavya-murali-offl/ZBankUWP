using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using ZBank.Services;

namespace ZBank.ViewModel.VMObjects 
{
    public class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void OnPropertyChanged([CallerMemberName] string propertyName = "", CoreDispatcher dispatcher = null)
        {
            if (PropertyChanged != null)
            {
                if (dispatcher == null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    if (dispatcher.HasThreadAccess)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }
                    else
                    {
                        await dispatcher.CallOnUIThreadAsync(
                             () =>
                             {
                                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                             });
                    }
                }
            }
        }

    }
}
