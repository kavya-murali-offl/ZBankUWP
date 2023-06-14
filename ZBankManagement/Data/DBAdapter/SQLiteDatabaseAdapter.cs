using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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



        private  SQLiteConnectionString GetConnectionString()
        {

            string databaseFileName = "BankDB.db3";
            string databaseLocation = Path.Combine(ApplicationData.Current.LocalFolder.Path, databaseFileName);
            return new SQLiteConnectionString(databaseLocation);

        }
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
