using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConn
{
    public partial class DBConnection
    {

        private string connectionStringDebug = ConfigurationManager.AppSettings["ConnectionStringDebug"];
        private string connectionStringRelease = ConfigurationManager.AppSettings["ConnectionStringRelease"];
        private string connectionString = ConfigurationManager.AppSettings["ConnectionStringRelease"];
        private SqlConnection connection;

        public DBConnection()
        {
            if (Debugger.IsAttached)
            {
                this.connection = new SqlConnection(connectionStringDebug);
                connectionString = ConfigurationManager.AppSettings["ConnectionStringDebug"];
            }
            else
            {
                this.connection = new SqlConnection(connectionStringRelease);
                connectionString = ConfigurationManager.AppSettings["ConnectionStringRelease"];
            }
        }


        public void OpenConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }

        public int ExecuteInsert(string insertQuery)
        {
            int rowsAffected = 0;

            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return rowsAffected;
        }

        public IEnumerable<T> ExecuteQuery<T>(string query, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<T>(query, parameters);
            }
        }

        public void InsertEntity<T>(T entity, string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 生成插入SQL语句，假设实体属性与表列名相同
                string columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
                string parameters = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name));
                string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                connection.Execute(sql, entity, commandType: CommandType.Text);
            }
        }

        public void Update(string SQL)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(SQL, commandType: CommandType.Text);
            }
        }


        public void DeleteEntity<T>(string columnName, object columnValue, string columnName2, object columnValue2, string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 生成列名和参数列表
                string sql = $"DELETE FROM {tableName} WHERE {columnName} = '{columnName}' And {columnName2} = '{columnName2}'";
                connection.Execute(sql, commandType: CommandType.Text);
            }
        }
    }
}