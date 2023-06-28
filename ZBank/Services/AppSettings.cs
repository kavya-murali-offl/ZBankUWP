using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZBank.Services
{
    internal class AppSettings
    {
        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }
        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }

        public string CustomerID
        {
            get => GetSettingsValue("CustomerID", default(string));
            set => LocalSettings.Values["CustomerID"] = value;
        }

        static AppSettings()
        {
            Current = new AppSettings();
        }

        static public AppSettings Current { get; }


    }
}
