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
            Database.SetInitializer<AppDbContext>(null);
        }

        public DbSet<nhomDTO> Nhoms { get; set; }
        public DbSet<loaiDTO> Loais { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<loaiDTO>()
                .HasRequired(l => l.Nhom)      
                .WithMany(n => n.Loais)        
                .HasForeignKey(l => l.MaNhom)  
                .WillCascadeOnDelete(false);  
                                               
        }
    }
}