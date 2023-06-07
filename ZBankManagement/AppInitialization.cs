using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZBank.DatabaseAdapter;
using ZBank.Dependencies;
using ZBank.ZBankManagement.DataLayer.DBHandler;

namespace ZBank.ZBankManagement
{
    public class AppInitialization
    {
        private static AppInitialization _appInitialization;
        private readonly IDBInitializationHandler _dbInitHandler;

        private AppInitialization() {
            _dbInitHandler = DependencyContainer.ServiceProvider.GetRequiredService<IDBInitializationHandler>(); ;
        }

        public static AppInitialization GetInstance()
        {
            if(_appInitialization == null) { 
                _appInitialization = new AppInitialization();
            }
            return _appInitialization;
        }



        public void InitializeDB()
        {
            _dbInitHandler.CreateTables();
            _dbInitHandler.PopulateData();
        }
    }
}
