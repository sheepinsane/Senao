using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConn
{
    public partial class DBConnection
    {

        private string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private SqlConnection connection;

        public DBConnection()
        {
            this.connection = new SqlConnection(connectionString);
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

        public IEnumerable<T> ExecuteQuery<T>(string query, object parameters = null)
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<T>(query, parameters);
            }
        }
    }
}