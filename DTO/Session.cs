using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO
{
    public static class Session
    {
        public static taikhoanDTO TaiKhoanHienTai;

        public static nhanVienDTO NhanVienHienTai;
        public static BindingList<phanquyenDTO> QuyenHienTai = new BindingList<phanquyenDTO>();
    }
}