using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class phanquyenDTO
    {
        public int MaVaiTro { get; set; }
        public int MaQuyen { get; set; }

        public int CAN_READ { get; set; }
        public int CAN_CREATE { get; set; }
        public int CAN_UPDATE { get; set; }
        public int CAN_DELETE { get; set; }
        public string TenQuyen { get; set; }
    }
}