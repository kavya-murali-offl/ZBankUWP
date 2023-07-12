using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBank.Services;
using ZBank.View;
using ZBank.ViewModel.VMObjects;

namespace ZBank.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public IView View { get; set; }

        private string _title = string.Empty;

        public string Title {
            get => _title;
            set => Set(ref _title, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                if (View?.Dispatcher == null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    if (View.Dispatcher.HasThreadAccess)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }
                    else
                    {
                        await View.Dispatcher.CallOnUIThreadAsync(
                             () =>
                            {
                                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                            });
                    }
                }
            }
        }

        public void ValidateObject(ObservableDictionary<string, string> FieldErrors, Type type, List<string> fieldsToValidate, object objectToCompare)
        {
            foreach (var field in fieldsToValidate)
            {
                var property = type.GetProperty(field);
                var value = property.GetValue(objectToCompare);
                ValidateField(FieldErrors, property.Name, value);
            }
        }


        public void ValidateField(ObservableDictionary<string, string> FieldErrors, string field, object value)
        {
                if (value is null || string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    FieldErrors[field] = $"{field} is required.";
                }
                else
                {
                    FieldErrors[field] = string.Empty;
                    if ( field == "Amount" ||  field == "Balance")
                    {
                        if (decimal.TryParse(value.ToString(), out decimal amountInDecimal))
                        {
                            if (amountInDecimal <= 0)
                            {
                                FieldErrors[field] = "Amount should be greater than zero";
                            }
                            else
                            {
                                FieldErrors[field] = string.Empty;
                            }
                        }
                        else
                        {
                            FieldErrors[field] = "Please enter a valid Amount";
                        }
                    }
                }
        }

        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

    }
}
