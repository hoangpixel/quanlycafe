using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTO
{
    [Table("nhom")]
    public class nhomDTO
    {
        [Key]
        public int MaNhom { get; set; }
        public string TenNhom { get; set; }
        public int? TrangThai { get; set; }

        public virtual ICollection<loaiDTO> Loais { get; set; }
    }
}