using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DAO.CONFIG
{
    internal class DBConnect
    {
        private static string server = "localhost";
        private static string database = "quan_cafe";
        private static string user = "root";
        private static string password = "";
        private static string port = "3306";

        private static string connectionString =
            $"Server={server};Database={database};User Id={user};Password={password};Port={port};SslMode=none;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                Console.WriteLine("Kết nối MySQL thành công!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi kết nối database : ", ex);
            }
            return conn;
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi khi đóng kết nối: " + ex.Message);
            }
        }
    }
}