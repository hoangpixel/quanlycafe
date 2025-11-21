using System.Data.Entity;
using System.Data.Entity.Infrastructure; // Thư viện cấu trúc EF
using MySql.Data.EntityFramework; // Thư viện MySQL

namespace DAO
{
    // 1. Tạo một bộ "thông dịch viên" giả
    // Khi EF hỏi: "Database phiên bản mấy?", thằng này sẽ trả lời: "5.7" (để qua mặt kiểm duyệt)
    public class MySqlVersionFix : IManifestTokenResolver
    {
        public string ResolveManifestToken(System.Data.Common.DbConnection connection)
        {
            // Trả về cứng số 5.7 để đánh lừa nó
            return "5.7";
        }
    }

    // 2. Tạo bộ cấu hình EF sử dụng cái thông dịch viên ở trên
    public class MySqlConfiguration : MySqlEFConfiguration
    {
        public MySqlConfiguration()
        {
            // Ép EF dùng bộ giải mã phiên bản của mình
            SetManifestTokenResolver(new MySqlVersionFix());
        }
    }
}