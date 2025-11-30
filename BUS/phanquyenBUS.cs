using DAO;
using DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class phanquyenBUS
    {
        private phanquyenDAO data = new phanquyenDAO();
        public static BindingList<phanquyenDTO> ds = new BindingList<phanquyenDTO>();
        public static BindingList<phanquyenDTO> dsHienThi = new BindingList<phanquyenDTO>();

        public BindingList<phanquyenDTO> LayDanhSach()
        {
            // Load lại nếu chưa có dữ liệu
            if (dsHienThi == null || dsHienThi.Count == 0)
            {
                dsHienThi = data.LayDanhSach();
            }
            return dsHienThi;
        }

        public BindingList<phanquyenDTO> LayChiTietQuyenTheoVaiTro(int maVaiTro)
        {
            // Logic cache cho ds chi tiết
            if (ds == null || ds.Count == 0 || (ds.Count > 0 && ds[0].MaVaiTro != maVaiTro))
            {
                ds = data.LayChiTietQuyenTheoVaiTro(maVaiTro);
            }
            return ds;
        }

        public bool LuuPhanQuyen(int maVaiTro, BindingList<phanquyenDTO> dsQuyenMoi)
        {
            // 1. Thao tác Database (Giữ nguyên)
            bool kqXoa = data.XoaToanBoQuyenCuaVaiTro(maVaiTro);
            if (!kqXoa) return false;

            bool kqThem = true;
            foreach (var pq in dsQuyenMoi)
            {
                pq.MaVaiTro = maVaiTro;
                if (!data.ThemPhanQuyen(pq))
                {
                    kqThem = false;
                    break;
                }
            }

            // 2. Cập nhật Cache nếu lưu thành công
            if (kqThem)
            {
                // [Bước A]: Chuẩn bị Tên Quyền (Giữ nguyên)
                Dictionary<int, string> mapTenQuyen = new Dictionary<int, string>();
                if (dsHienThi != null)
                {
                    foreach (var item in dsHienThi)
                    {
                        if (!mapTenQuyen.ContainsKey(item.MaQuyen) && !string.IsNullOrEmpty(item.TenQuyen))
                            mapTenQuyen.Add(item.MaQuyen, item.TenQuyen);
                    }
                }

                // [Bước B]: Cập nhật ds (Cache chi tiết) - Xóa đi thêm lại cho sạch
                ds.Clear();
                foreach (var item in dsQuyenMoi)
                {
                    if (mapTenQuyen.ContainsKey(item.MaQuyen))
                        item.TenQuyen = mapTenQuyen[item.MaQuyen];
                    ds.Add(item);
                }

                // [Bước C]: Cập nhật dsHienThi (SỬA ĐOẠN NÀY ĐỂ GIỮ THỨ TỰ)
                if (dsHienThi != null)
                {
                    // Duyệt qua danh sách mới vừa lưu (ds)
                    foreach (var itemMoi in ds)
                    {
                        // Tìm xem trong danh sách hiển thị đã có dòng này chưa
                        var itemCu = dsHienThi.FirstOrDefault(x => x.MaVaiTro == itemMoi.MaVaiTro && x.MaQuyen == itemMoi.MaQuyen);

                        if (itemCu != null)
                        {
                            // 1. NẾU CÓ RỒI -> Cập nhật đè lên chính vị trí đó (Để giữ thứ tự)
                            int index = dsHienThi.IndexOf(itemCu);
                            dsHienThi[index] = itemMoi; // Gán object mới vào vị trí cũ => Grid tự refresh
                        }
                        else
                        {
                            // 2. NẾU CHƯA CÓ -> Thêm mới vào cuối
                            dsHienThi.Add(itemMoi);
                        }
                    }

                    // 3. Xóa những dòng dư thừa (Những dòng có trên lưới nhưng không còn trong ds mới)
                    for (int i = dsHienThi.Count - 1; i >= 0; i--)
                    {
                        var item = dsHienThi[i];
                        // Chỉ xét những dòng thuộc vai trò đang sửa
                        if (item.MaVaiTro == maVaiTro)
                        {
                            // Nếu mã quyền này không tồn tại trong danh sách mới -> Xóa
                            bool conTonTai = ds.Any(x => x.MaQuyen == item.MaQuyen);
                            if (!conTonTai)
                            {
                                dsHienThi.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            return kqThem;
        }
    }
}