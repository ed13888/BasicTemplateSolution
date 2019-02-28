using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc.SQL
{
    /// <summary>Dapper帮助类</summary>
    public static class MySqlHelper
    {
        public static T ExecuteScalar<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName))
                return connection.ExecuteScalar<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static int Execute(string storedProcName, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection((string)null))
                return connection.Execute(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static async Task<int> ExecuteAsync(
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            int num;
            using (MySqlConnection conn = DBFactory.CreateMySqlConnection((string)null))
                num = await conn.ExecuteAsync(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
            return num;
        }

        public static IEnumerable<T> Query<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName))
                return connection.Query<T>(storedProcName, param, (IDbTransaction)null, true, new int?(), new CommandType?(commandType));
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            IEnumerable<T> objs;
            using (MySqlConnection conn = DBFactory.CreateMySqlConnection(databaseName))
                objs = await conn.QueryAsync<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
            return objs;
        }

        public static T FirstOrDefault<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName))
                return connection.QueryFirstOrDefault<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static async Task<T> FirstOrDefaultAsync<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            T obj;
            using (MySqlConnection conn = DBFactory.CreateMySqlConnection(databaseName))
                obj = await conn.QueryFirstOrDefaultAsync<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
            return obj;
        }

        public static SqlMapper.GridReader QueryMultiple(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.QueryMultiple(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static Task<SqlMapper.GridReader> QueryMultipleAsync(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection.QueryMultipleAsync(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static T SingleOrDefault<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName))
                return connection.QuerySingleOrDefault<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
        }

        public static async Task<T> SingleOrDefaultAsync<T>(
          string databaseName,
          string storedProcName,
          object param = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            T obj;
            using (MySqlConnection conn = DBFactory.CreateMySqlConnection(databaseName))
                obj = await conn.QuerySingleOrDefaultAsync<T>(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType));
            return obj;
        }

        public static void ExecuteReader(
          string databaseName,
          string storedProcName,
          object param = null,
          Action<IDataReader> executed = null,
          CommandType commandType = CommandType.StoredProcedure)
        {
            using (MySqlConnection connection = DBFactory.CreateMySqlConnection(databaseName))
            {
                using (IDataReader dataReader = connection.ExecuteReader(storedProcName, param, (IDbTransaction)null, new int?(), new CommandType?(commandType)))
                {
                    if (executed != null)
                        executed(dataReader);
                    if (dataReader.IsClosed)
                        return;
                    dataReader.Close();
                }
            }
        }
    }
}
