using System.Data.Entity;
using MySql.Data.EntityFramework;
using DTO;

namespace DAO
{
    [DbConfigurationType(typeof(MySqlConfiguration))]
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("server=localhost;port=3306;database=quan_cafe;uid=root;password=;SslMode=None;")
        {
            // Thêm dòng này để nó tự tạo Database nếu chưa có (hoặc check kết nối)
            Database.SetInitializer<AppDbContext>(null);
        }

        public DbSet<nhomDTO> Nhoms { get; set; }
    }
}