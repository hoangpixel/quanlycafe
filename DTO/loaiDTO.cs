using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DTO
{
    [Table("loai")]
    public class loaiDTO
    {
        [Key]
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }

        public int MaNhom { get; set; }
        public int TrangThai { get; set; }
        public loaiDTO() { }

        [ForeignKey("MaNhom")]
        public virtual nhomDTO Nhom { get; set; }
    }
}
