﻿using System;
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
using Windows.UI.Xaml;
using ZBank.AppEvents;
using ZBank.Services;
using ZBank.View;
using ZBank.View.UserControls;
using ZBank.ViewModel.VMObjects;

namespace ZBank.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        public IView View { get; set; }

        virtual public void Merge(ObservableObject source) { }
       
        protected bool IsBusy { get; set; } 

        public async Task SetBusy(bool busy) {
            if (busy)
            {
               await DialogService.ShowContentAsync(View, new LoadingScreen(), "", Window.Current.Content.XamlRoot);
            }
            else
            {
                await View.Dispatcher.CallOnUIThreadAsync(() =>
                {
                    ViewNotifier.Instance.OnCloseDialog();
                });
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
                OnPropertyChanged(propertyName, View?.Dispatcher);
                return true;
            }
            return false;
        }
    }
}
