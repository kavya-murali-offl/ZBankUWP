
using ZBankManagement.DataManager;
using ZBankManagement.Interface;
using Microsoft.Extensions.DependencyInjection;
using ZBank.DatabaseAdapter;
using ZBank.DatabaseHandler;
using ZBank.ZBankManagement.DataLayer.DataManager.Contracts;
using ZBank.ZBankManagement.DataLayer.DataManager;
using ZBank.AppEvents;
using ZBankManagement.Data.DataManager.Contracts;
using ZBankManagement.Data.DataManager;

namespace ZBank.Dependencies
{
    static class DependencyContainer
    {
        //465419
        static DependencyContainer()
        {
            ServiceProvider = 
                new ServiceCollection()
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

        private static IServiceCollection ConfigureCardServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGetCardDataManager, GetCardDataManager>();
            serviceCollection.AddScoped<IInsertCardDataManager, InsertCardDataManager>();
            serviceCollection.AddScoped<IUpdateCardDataManager, UpdateCardDataManager>();
            serviceCollection.AddScoped<IInitializeAppDataManager, InitializeAppDataManager>();
            serviceCollection.AddScoped<IResetPinDataManager, ResetPinDataManager>();
            serviceCollection.AddScoped<ISignupUserDataManager, SignupUserDataManager>();
            serviceCollection.AddScoped<ICloseDepositDataManager, CloseDepositDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureCustomerServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILoginCustomerDataManager, LoginCustomerDataManager>();
            serviceCollection.AddScoped<IUpdateCustomerDataManager, UpdateCustomerDataManager>();
            return serviceCollection;
        }

        private static IServiceCollection ConfigureAccountServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IInsertAccountDataManager, InsertAccountDataManager>();
            serviceCollection.AddScoped<IGetAccountDataManager, GetAccountDataManager>();
            serviceCollection.AddScoped<IUpdateAccountDataManager, UpdateAccountDataManager>();
            serviceCollection.AddScoped<IGetDashboardDataDataManager, GetDashboardDataDataManager>();
            serviceCollection.AddScoped<IGetBeneficiaryDataManager, GetBeneficiaryDataManager>();
            serviceCollection.AddScoped<IInsertBeneficiaryDataManager, InsertBeneficiaryDataManager>();
            serviceCollection.AddScoped<IUpdateBeneficiaryDataManager, UpdateBeneficiaryDataManager>();
            serviceCollection.AddScoped<IGetBranchDetailsDataManager, GetBranchDetailsDataManager>();
            serviceCollection.AddScoped<ITransferAmountDataManager, TransferAmountDataManager>();
            serviceCollection.AddScoped<IDeleteBeneficiaryDataManager, DeleteBeneficiaryDataManager>();
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
