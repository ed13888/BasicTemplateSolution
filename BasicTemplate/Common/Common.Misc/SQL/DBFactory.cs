using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class DBFactory
    {
        private static int TIMEOUTSECONDS = 180;

        public static string GetConnctString(string databaseName)
        {
            string currentAssemblyGuid = DBFactory.GetCurrentAssemblyGuid();
            string empty = string.Empty;
            //return EncryptHelper.DecryptConnctString(!string.IsNullOrWhiteSpace(databaseName) ? ConfigurationManager.ConnectionStrings[databaseName].ConnectionString : ConfigurationManager.ConnectionStrings["Cash"].ConnectionString, currentAssemblyGuid);
            return !string.IsNullOrWhiteSpace(databaseName) ? ConfigurationManager.ConnectionStrings[databaseName].ConnectionString : ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        private static string GetCurrentAssemblyGuid()
        {
            object[] customAttributes = typeof(DBFactory).Assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (((IEnumerable<object>)customAttributes).Any<object>())
                return ((GuidAttribute)((IEnumerable<object>)customAttributes).First<object>()).Value;
            return "";
        }

        public static Database Create(string databaseName)
        {
            return (Database)new SqlDatabase(DBFactory.GetConnctString(databaseName));
        }

        public static DbCommand CreateDbCommand(
          Database db,
          string storedProcName,
          params object[] paramValues)
        {
            DbCommand dbCommand = paramValues == null || paramValues.Length == 0 ? db.GetStoredProcCommand(storedProcName) : db.GetStoredProcCommand(storedProcName, paramValues);
            dbCommand.CommandTimeout = DBFactory.TIMEOUTSECONDS;
            return dbCommand;
        }

        public static DbCommand CreateDbCommandSqlText(
          Database db,
          string sqlText,
          IEnumerable<IDataParameter> paramValues)
        {
            DbCommand sqlStringCommand = db.GetSqlStringCommand(sqlText);
            sqlStringCommand.CommandTimeout = DBFactory.TIMEOUTSECONDS;
            foreach (IDataParameter paramValue in paramValues)
                sqlStringCommand.Parameters.Add((object)paramValue);
            return sqlStringCommand;
        }

        public static DbCommand CreateDbCommandSqlText(Database db, string sqlText)
        {
            DbCommand sqlStringCommand = db.GetSqlStringCommand(sqlText);
            sqlStringCommand.CommandTimeout = DBFactory.TIMEOUTSECONDS;
            return sqlStringCommand;
        }

        public static SqlConnection CreateConnection(string databaseName)
        {
            string connctString = DBFactory.GetConnctString(databaseName);
            if (string.IsNullOrEmpty(connctString))
                throw new ArgumentNullException(connctString, "连接字符串不允许为空");
            SqlConnection sqlConnection = new SqlConnection(connctString);
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
            return sqlConnection;
        }

        public static MySqlConnection CreateMySqlConnection(string databaseName)
        {
            string connctString = DBFactory.GetConnctString(databaseName);
            if (string.IsNullOrEmpty(connctString))
                throw new ArgumentNullException(connctString, "连接字符串不允许为空");
            MySqlConnection sqlConnection = new MySqlConnection(connctString);
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
            return sqlConnection;
        }
    }
}
