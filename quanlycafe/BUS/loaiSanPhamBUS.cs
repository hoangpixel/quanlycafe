using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quanlycafe.DAO;
using quanlycafe.DTO;

namespace quanlycafe.BUS
{
    internal class loaiSanPhamBUS
    {
        public static List<loaiDTO> ds;

        public List<loaiDTO> layDanhSachLoai()
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO(); 
            return data.docDanhSachLoai();             
        }


    }
}
