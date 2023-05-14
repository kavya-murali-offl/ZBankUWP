
using ZBankManagement.DataManager;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.DatabaseAdapter;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DataLayer.DBHandler;
using ZBank.ZBankManagement;
using ZBank.ZBankManagement.AppEvents;

namespace ZBank.Dependencies
{
    public static class DependencyContainer
    {

        static DependencyContainer()
        {
            ServiceProvider = 
                new ServiceCollection()
                //.ConfigureServices()
                .ConfigureDBAdapter()
                .ConfigureCustomerServices()
                .ConfigureAccountServices()
                .ConfigureTransactionServices()
                .ConfigureCardServices()
                .BuildServiceProvider();
        }

        public static ServiceProvider ServiceProvider { get; set; }

        private static IServiceCollection ConfigureDBAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDatabaseAdapter, SQLiteDatabaseAdapter>();
            serviceCollection.AddSingleton<IDBHandler, DBHandler>();
            serviceCollection.AddSingleton<IDBInitializationHandler, DBInitializationHandler>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureCardServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetCardDataManager, GetCardDataManager>();
            serviceCollection.AddScoped<IInsertCardDataManager, InsertCardDataManager>();
            serviceCollection.AddScoped<IUpdateCardDataManager, UpdateCardDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureCustomerServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoginCustomerDataManager, LoginCustomerDataManager>();
            //serviceCollection.AddScoped<IInsertCustomerDataManager, InsertCustomerDataManager>();
            serviceCollection.AddScoped<IUpdateCustomerDataManager, UpdateCustomerDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureAccountServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsertAccountDataManager, InsertAccountDataManager>();
            serviceCollection.AddScoped<IGetAccountDataManager, GetAccountDataManager>();
            serviceCollection.AddScoped<IUpdateAccountDataManager, UpdateAccountDataManager>();
            serviceCollection.AddSingleton<ViewNotifier>();
            return serviceCollection;
        }
    
        private static IServiceCollection ConfigureTransactionServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsertTransactionDataManager, InsertTransactionDataManager>();
            serviceCollection.AddScoped<IGetTransactionDataManager, GetTransactionDataManager>();
            return serviceCollection;
        }
    }
}
