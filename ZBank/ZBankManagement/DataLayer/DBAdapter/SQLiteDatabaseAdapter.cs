using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.DatabaseAdapter
{
    public class SQLiteDatabaseAdapter : IDatabaseAdapter
    {
        private static IDatabaseAdapter _databaseAdapter;

        public SQLiteDatabaseAdapter()
        {
            SQLiteConnectionString connectionString = GetConnectionString();
            Connection = new SQLiteAsyncConnection(connectionString);
        }

        public static IDatabaseAdapter GetInstance()
        {
            if(_databaseAdapter == null)
            {
                _databaseAdapter = new SQLiteDatabaseAdapter(); 
            }
            return _databaseAdapter;
        }

        private SQLiteAsyncConnection Connection { get; set; }

        private SQLiteConnectionString GetConnectionString()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string databasePath = Path.Combine(localFolder.Path, "BankDB.db3");
            return new SQLiteConnectionString(databasePath, true);
        }

        public async Task CreateTable<T>() where T : new()
        {
            await Connection.CreateTableAsync<T>();
        }

        public Task<int> Insert<T>(T instance, Type insertionType=null)
        {
           return insertionType == null ?  Connection.InsertAsync(instance) : Connection.InsertAsync(instance, insertionType);
        }

        public Task<int> Update<T>(T instance)
        {
            return Connection.InsertOrReplaceAsync(instance, typeof(T));
        }

        public AsyncTableQuery<T> GetAll<T>() where T : new()
        {
            return Connection.Table<T>();
        }

        public async Task<T> GetScalar<T>(string query, params object[] args)
        {
            return await Connection.ExecuteScalarAsync<T>(query, args);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new() => await Connection.QueryAsync<T>(query, args);

        public Task RunInTransaction(Action action)
        {
            return Connection.RunInTransactionAsync(tran =>
            {
                action();
            });
        }
    }
}
