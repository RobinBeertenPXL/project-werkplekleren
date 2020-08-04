using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXLData
{    
    public static class DatabaseConnection
    {
        public static SqlConnection sqlConnection;
        public static string connectionString;
        public static SqlTransaction sqlTransaction;
    }
    public static class DatabaseLauncher
    {       
        public static void Start()
        {
            if(DatabaseConnection.sqlConnection == null)
                Database.OpenConnection();
            else if(DatabaseConnection.sqlConnection.State != ConnectionState.Open)
                Database.OpenConnection();
        }
        public static void Stop()
        {
            Database.CloseConnection();
        }
        public static void StartWithoutTransaction()
        {
            Database.OpenConnectionNoTrans();
        }
        public static void StopWithoutTransaction()
        {
            Database.CloseConnectionNoTrans();
        }
    }
    static class Database
    {
        public static bool StartTransaction=false;       
        internal static void OpenConnection()
        {
            StartTransaction = true;
            DatabaseConnection.sqlConnection = new SqlConnection(DatabaseConnection.connectionString);
            DatabaseConnection.sqlConnection.Open();
            DatabaseConnection.sqlTransaction = DatabaseConnection.sqlConnection.BeginTransaction();
        }
        internal static void CloseConnection()
        {           
            if (DatabaseConnection.sqlConnection != null)
            {
                if(DatabaseConnection.sqlConnection.State == ConnectionState.Open)
                {
                    if(DatabaseConnection.sqlTransaction.Connection != null)
                        DatabaseConnection.sqlTransaction.Commit();
                    DatabaseConnection.sqlConnection.Close();
                }
            }
            StartTransaction = false;
        }
        internal static void OpenConnectionNoTrans()
        {
            DatabaseConnection.sqlConnection = new SqlConnection(DatabaseConnection.connectionString);
            DatabaseConnection.sqlConnection.Open();
        }
        internal static void CloseConnectionNoTrans()
        {
            if(DatabaseConnection.sqlConnection.State == ConnectionState.Open)
                DatabaseConnection.sqlConnection.Close();
        }
    }
    public class DatabaseCommand
    {
        protected int CreateDBRecord(string sqlCommText)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = DatabaseConnection.sqlConnection;
            sqlCommand.Transaction = DatabaseConnection.sqlTransaction;
            sqlCommText += ";SELECT scope_identity();";
            sqlCommand.CommandText = sqlCommText;
            string sqlValue=sqlCommand.ExecuteScalar().ToString();
            int newID = -1;
            int.TryParse(sqlValue, out newID);
            return newID;
        }
        protected void UpdateDBRecord(string sqlCommText)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = DatabaseConnection.sqlConnection;
            sqlCommand.Transaction = DatabaseConnection.sqlTransaction;
            sqlCommand.CommandText = sqlCommText;
            sqlCommand.ExecuteNonQuery();
        }
        protected void DeleteDBRecord(string sqlCommText)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = DatabaseConnection.sqlConnection;
            sqlCommand.Transaction = DatabaseConnection.sqlTransaction;
            sqlCommand.CommandText = sqlCommText;
            sqlCommand.ExecuteNonQuery();
        }
        protected DataTable GetExecuteReader(string query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, DatabaseConnection.sqlConnection);
            if(Database.StartTransaction)
                sqlCommand.Transaction = DatabaseConnection.sqlTransaction;            
            DataTable dt=new DataTable();
            dt.Load(sqlCommand.ExecuteReader());
            return dt;
        }
    }
}
