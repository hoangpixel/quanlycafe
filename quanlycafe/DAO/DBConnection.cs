using System;
using System.Data;
using System.Data.SqlClient; // dùng cho SQL Server
// Nếu bạn dùng MySQL thì cần thêm MySql.Data (NuGet)
using MySql.Data.MySqlClient;

namespace quanlycafe.config
{
    internal class DBConnect
    {
        private static string server = "localhost";
        private static string database = "quan_cafe"; // tên DB của bạn
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
                conn.Open();
                Console.WriteLine("Kết nối MySQL thành công!");
            }
            catch (MySqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Lỗi kết nối cơ sở dữ liệu:\n" + ex.Message,
                    "Lỗi", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );
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
