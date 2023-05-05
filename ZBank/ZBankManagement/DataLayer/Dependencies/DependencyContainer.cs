﻿
using BankManagementDB.DataManager;
using BankManagementDB.Events;
using BankManagementDB.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.DatabaseAdapter;
using ZBank.DatabaseHandler;

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
            return serviceCollection;
        }

        //private static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        //{
        //    Config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    return serviceCollection;
        //}

        private static IServiceCollection ConfigureCardServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetCardDataManager, GetCardDataManager>();
            serviceCollection.AddScoped<IInsertCardDataManager, InsertCardDataManager>();
            serviceCollection.AddScoped<IInsertCreditCardDataManager, InsertCreditCardDataManager>();
            serviceCollection.AddScoped<IInsertDebitCardDataManager, InsertDebitCardDataManager>();
            serviceCollection.AddScoped<IUpdateCardDataManager, UpdateCardDataManager>();
            serviceCollection.AddScoped<IUpdateCreditCardDataManager, UpdateCreditCardDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureCustomerServices(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddScoped<IGetCustomerCredentialsDataManager, GetCustomerCredentialsDataManager>();
            serviceCollection.AddScoped<IInsertCredentialsDataManager, InsertCredentialsDataManager>();
            serviceCollection.AddScoped<IGetCustomerDataManager, GetCustomerDataManager>();
            serviceCollection.AddScoped<IInsertCustomerDataManager, InsertCustomerDataManager>();
            serviceCollection.AddScoped<IUpdateCustomerDataManager, UpdateCustomerDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureAccountServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsertAccountDataManager, InsertAccountDataManager>();
            serviceCollection.AddScoped<IGetAccountDataManager, GetAccountDataManager>();
            serviceCollection.AddScoped<IUpdateAccountDataManager, UpdateAccountDataManager>();
            //serviceCollection.AddSingleton<IAccountFactory, AccountFactory>();
            serviceCollection.AddSingleton<AppEvents>();
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
