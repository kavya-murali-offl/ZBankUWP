using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using ZBankManagement.Data;

namespace ZBank.DatabaseAdapter
{
    class SQLiteDatabaseAdapter : IDatabaseAdapter
    {
        private static IDatabaseAdapter _databaseAdapter;

        public SQLiteDatabaseAdapter()
        {
            SQLiteConnectionString connectionString = new SQLiteConnectionString(Config.DatabasePath);
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



        public async Task CreateTable<T>() where T : new()
        {
            await Connection.CreateTableAsync<T>();
        }

        public Task<int> Insert<T>(T instance, Type insertionType=null)
        {
           return insertionType == null ?  Connection.InsertAsync(instance) : Connection.InsertAsync(instance, insertionType);
        }

        public Task<int> InsertAll<T>(IEnumerable<T> list)
        {
            return Connection.InsertAllAsync(list);
        }

        public Task<int> Update<T>(T instance, Type type = null)
        {
            return Connection.UpdateAsync(instance, type);
        }

        public AsyncTableQuery<T> GetAll<T>() where T : new()
        {
            return Connection.Table<T>();
        }

        public async Task<T> GetScalar<T>(string query, params object[] args)
        {
            return await Connection.ExecuteScalarAsync<T>(query, args);
        }

        public async Task<int> Execute(string query, params object[] args)
        {
            return await Connection.ExecuteAsync(query, args);
        }

        public async Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new() => await Connection.QueryAsync<T>(query, args);

        public async Task RunInTransactionAsync(Func<Task> action)
        {
            SQLiteConnectionWithLock conn = Connection.GetConnection();
            conn.BeginTransaction();
            try
            {
                await action();
                conn.Commit();
            }
            catch (Exception)
            {
                conn.Rollback();
                throw;
            }
        }
    }
}
