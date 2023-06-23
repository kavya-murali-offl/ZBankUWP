using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZBankManagement.Data
{
    internal class Config
    {
        internal static string DatabasePath
        {
            get
            {
                return Path.Combine(ApplicationData.Current.LocalFolder.Path, "BankDB.db3");
            }
        }
    }
}
