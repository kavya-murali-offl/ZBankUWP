﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZBank.Entities;

namespace ZBank.DatabaseAdapter
{
    public class SQLiteDatabaseAdapter : IDatabaseAdapter
    {

        public SQLiteDatabaseAdapter()
        {
            SQLiteConnectionString connectionString = GetConnectionString();
            Connection = new SQLiteAsyncConnection(connectionString);
        }

        private SQLiteAsyncConnection Connection { get; set; }

        private SQLiteConnectionString GetConnectionString(){
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string databasePath = Path.Combine(localFolder.Path, "BankDB.db3");
            return new SQLiteConnectionString(databasePath, true, key: "pass");
        }

        public async Task CreateTable<T>() where T : new() => await Connection.CreateTableAsync<T>();

        public async Task<bool> Insert<T>(T instance) => await Connection.InsertAsync(instance, typeof(T)) > 0;

        public async Task<bool> Update<T>(T instance) => await Connection.InsertOrReplaceAsync(instance, typeof(T)) > 0;

        public AsyncTableQuery<T> GetAll<T>() where T : new() => Connection.Table<T>();
        
        public async Task<T> GetScalar<T>(string query, params object[] args) => await Connection.ExecuteScalarAsync<T>(query, args);

        public async Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new() => await Connection.QueryAsync<T>(query, args);

        public async Task RunInTransaction(Action action)
        {
            try
            {
                await Connection.RunInTransactionAsync(tran =>
                {
                    action();
                });
            }catch(Exception err)
            {
                throw (err);
            }
        }
    }
}
