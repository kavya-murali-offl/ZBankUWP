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

        public ObservableDictionary<string, string> ValidateObject(ObservableDictionary<string, string> FieldErrors, Type type, List<string> fieldsToValidate, object objectToCompare)
        {
            foreach (var field in fieldsToValidate)
            {
                var property = type.GetProperty(field);
                var value = property.GetValue(objectToCompare);
                return ValidateField(FieldErrors, property.Name, value);
            }

            return FieldErrors;
            //    if (value is null || string.IsNullOrEmpty(value.ToString()) || string.IsNullOrWhiteSpace(value.ToString()))
            //    {
            //        FieldErrors[property.Name] = $"{property.Name} is required.";
            //    }
            //    else
            //    {
            //        FieldErrors[property.Name] = string.Empty;
            //        if (property.Name == "Amount" || property.Name == "Balance")
            //        {
            //            if (decimal.TryParse(value.ToString(), out decimal amountInDecimal))
            //            {
            //                if (amountInDecimal <= 0)
            //                {
            //                    FieldErrors["Amount"] = "Amount should be greater than zero";
            //                }
            //                else
            //                {
            //                    FieldErrors["Amount"] = string.Empty;
            //                }
            //            }
            //            else
            //            {
            //                FieldErrors["Amount"] = "Please enter a valid Amount";
            //            }
            //        }
            //    }
            //}
            //return FieldErrors;
        }


        public ObservableDictionary<string, string> ValidateField(ObservableDictionary<string, string> FieldErrors, string field, object value)
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
