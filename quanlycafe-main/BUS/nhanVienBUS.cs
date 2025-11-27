using DAO;
using DTO;
using System;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class nhanVienBUS
    {
        // Khởi tạo lớp DAO để giao tiếp với CSDL
        private nhanVienDAO dao = new nhanVienDAO();

     
        public BindingList<nhanVienDTO> LayDanhSach()
        {
            try
            {
                return dao.LayDanhSach();
            }
            catch (Exception ex)
            {
                // Có thể ghi log lỗi ở đây nếu cần
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tên nhân viên theo mã (Hỗ trợ hiển thị trên phiếu nhập)
        /// </summary>
        public string LayTenNhanVien(int maNV)
        {
            var list = dao.LayDanhSach();
            var nv = list.FirstOrDefault(x => x.MaNhanVien == maNV);
            return nv != null ? nv.HoTen : "Không xác định";
        }

        

    }
}