using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Markings.Model
{
    public static class Database
    {
        //private System.Data.SqlClient.SqlTransaction _context;
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static SqlConnection connect()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public static SqlTransaction transaction()
        {
            return connect().BeginTransaction();
        }

        // call a stored procedure
        public static DataTable call(string procedure, SqlParameter[] parameters = null)
        {
            SqlCommand command = new SqlCommand();

            command.Connection = connect();
            command.CommandText = procedure;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }

            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(result);
            command.Connection.Close();

            return result;
        }

        public enum QueryType
        {
            NonQuery,
            ScalarQuery,
            RowsetQuery,
        }

        public static string paginate(string sql, int offset, int pagesize, ref SqlParameter[] parameters)
        {
            if (pagesize > 0)
            {
                sql += " OFFSET @OFFSET ROWS FETCH NEXT @PAGESIZE ROWS ONLY";
                Array.Resize<System.Data.SqlClient.SqlParameter>(ref parameters, parameters.Length + 2);
                parameters[parameters.Length - 2] = new System.Data.SqlClient.SqlParameter("@OFFSET", offset);
                parameters[parameters.Length - 1] = new System.Data.SqlClient.SqlParameter("@PAGESIZE", pagesize);
            }
            return sql;
        }

        public static Object query_value(string sql)
        {
            return exec(sql, QueryType.ScalarQuery);
        }

        public static Object query_value(string sql, SqlParameter[] parameters)
        {
            return exec(sql, parameters, QueryType.ScalarQuery);
        }

        public static Object query(string sql)
        {
            return exec(sql, QueryType.RowsetQuery);
        }

        public static Object query(string sql, SqlParameter[] parameters)
        {
            return exec(sql, parameters, QueryType.RowsetQuery);
        }

        public static Object exec(string sql, QueryType type = QueryType.NonQuery)
        {
            //logger.Debug("Executing (DatabaseControl) SQL command with no parameters: " + sql);
            return exec(sql, null, type);
        }

        public static Object exec(string sql, SqlParameter[] parameters, QueryType type = QueryType.NonQuery)
        {
            Object result;
            SqlCommand command = new SqlCommand();

            command.Connection = connect();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
            }

            //logger.Debug("Executing (DatabaseControl) SQL command: " + command.CommandText);

            switch (type)
            {
                case QueryType.NonQuery:
                    result = command.ExecuteNonQuery();
                    break;
                case QueryType.ScalarQuery:
                    result = command.ExecuteScalar();
                    break;
                case QueryType.RowsetQuery:
                    result = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill((DataTable)result);
                    break;
                default:
                    result = null;
                    break;
            }

            command.Connection.Close();
            return result;
        }
    }
}