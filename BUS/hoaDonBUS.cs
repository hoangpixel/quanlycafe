using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BUS
{
    public class hoaDonBUS
    {
        private hoaDonDAO dao = new hoaDonDAO();
        public static BindingList<hoaDonDTO> ds = new BindingList<hoaDonDTO>();

        public BindingList<hoaDonDTO> LayDanhSach()
        {
            // Tạm thời bỏ if để đảm bảo luôn lấy dữ liệu mới nhất từ DB có điều kiện WHERE
            // if(ds == null || ds.Count == 0) 
            // {
            ds = dao.LayDanhSach();
            // }
            return ds;
        }

        // Dùng luôn cthoaDonDTO
        public int ThemHoaDon(hoaDonDTO hd, BindingList<cthoaDonDTO> dsChiTiet)
        {
            // Bước 1: Gọi DAO thêm xuống Database
            int newID = dao.Them(hd, dsChiTiet);

            // Bước 2: Nếu thêm thành công (ID > 0) -> Cập nhật vào list ds static
            if (newID > 0)
            {
                // Vì DTO truyền vào (hd) thường thiếu tên KH, tên NV (do chỉ có ID)
                // Nên ta gọi hàm lấy thông tin chi tiết để hiển thị lên Grid cho đẹp
                hoaDonDTO hoaDonMoi = dao.LayThongTinHoaDon(newID);

                if (hoaDonMoi != null)
                {
                    ds.Insert(0, hoaDonMoi);
                }
            }

            return newID;
        }

        public bool CapNhatTrangThai(int maHD, string trangThai)
            => dao.CapNhatTrangThai(maHD, trangThai);

        public bool KhoaHoaDon(int maHD) => dao.KhoaHoaDon(maHD);

        public hoaDonDTO TimTheoMa(int maHD) => dao.TimTheoMa(maHD);


        public bool XoaHoaDon(int maHD)
        {
            // Bước 1: Gọi DAO update xuống DB
            bool result = dao.UpdateTrangThai(maHD);

            // Bước 2: Nếu thành công -> Tìm và sửa trong list ds static
            if (result)
            {
                // Tìm hóa đơn trong danh sách đang hiển thị
                var item = ds.FirstOrDefault(x => x.MaHD == maHD);
                if (item != null)
                {
                    ds.Remove(item);
                }
            }

            return result;
        }


    }

}
