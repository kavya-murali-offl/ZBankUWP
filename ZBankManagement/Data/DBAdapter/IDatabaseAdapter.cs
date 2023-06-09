﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBank.DatabaseAdapter
{
    interface IDatabaseAdapter
    {
        Task CreateTable<T>() where T : new();

        AsyncTableQuery<T> GetAll<T>() where T : new();

        Task<T> GetScalar<T>(string query, params object[] args);

        Task<int> Insert<T>(T obj, Type insertionType=null);

        Task<int> InsertAll<T>(IEnumerable<T> list);

        Task<int> Update<T>(T obj, Type type = null);

        Task<IEnumerable<T>> Query<T>(string query, params object[] args) where T : new();

        Task<int> Execute(string query, params object[] args);

        Task RunInTransactionAsync(Func<Task> action);

        Task<int> Delete<T>(T instance);

    }
}
