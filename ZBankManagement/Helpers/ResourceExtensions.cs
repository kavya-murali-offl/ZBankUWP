using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace ZBankManagement.Helpers
{
    public static class ResourcesExtensions
    {

        public static string GetLocalized(this string resourceKey)
        {
            return _resLoader.GetString(resourceKey+".Text");
        }

        private static ResourceLoader _resLoader = new ResourceLoader();
    }
}
