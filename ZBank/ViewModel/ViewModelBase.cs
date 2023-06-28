using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ZBank.ViewModel.VMObjects;

namespace ZBank.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableDictionary<string, string> ValidateField(ObservableDictionary<string, string> FieldErrors, Type type, List<string> fieldsToValidate, object objectToCompare)
        {
                foreach(var field in fieldsToValidate)
                {
                    var property = type.GetProperty(field);
                    var value = property.GetValue(objectToCompare);
                    if (value is null || string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        FieldErrors[property.Name] = $"{property.Name} is required.";
                    }
                    else
                    {
                        FieldErrors[property.Name] = string.Empty;
                    }
                }
            return FieldErrors;
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
