using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BUS
{
    public class phanquyenBUS
    {
        private phanquyenDAO data = new phanquyenDAO();
        public static BindingList<phanquyenDTO> ds = new BindingList<phanquyenDTO>();
        public static BindingList<phanquyenDTO> dsHienThi = new BindingList<phanquyenDTO>();

        public BindingList<phanquyenDTO> LayDanhSach()
        {
            if (dsHienThi == null || dsHienThi.Count == 0)
            {
                dsHienThi = data.LayDanhSach();
            }
            return dsHienThi;
        }

        public BindingList<phanquyenDTO> LayChiTietQuyenTheoVaiTro(int maVaiTro)
        {
            // Luôn load lại từ DB nếu danh sách rỗng hoặc khác vai trò để đảm bảo dữ liệu tươi mới
            if (ds == null || ds.Count == 0 || (ds.Count > 0 && ds[0].MaVaiTro != maVaiTro))
            {
                ds = data.LayChiTietQuyenTheoVaiTro(maVaiTro);
            }
            return ds;
        }

        public bool LuuPhanQuyen(int maVaiTro, BindingList<phanquyenDTO> dsQuyenMoi)
        {
            // [BƯỚC 1]: QUAN TRỌNG NHẤT - COPY DANH SÁCH RA MỘT LIST RIÊNG
            // Để tránh việc ds.Clear() làm mất dữ liệu của dsQuyenMoi do trùng tham chiếu
            List<phanquyenDTO> listSaoChep = dsQuyenMoi.ToList();

            // [BƯỚC 2]: Thao tác Database (Xóa cũ -> Thêm mới)
            bool kqXoa = data.XoaToanBoQuyenCuaVaiTro(maVaiTro);
            if (!kqXoa) return false;

            bool kqThem = true;
            foreach (var pq in listSaoChep) // Duyệt trên list sao chép
            {
                pq.MaVaiTro = maVaiTro; // Đảm bảo mã vai trò đúng
                if (!data.ThemPhanQuyen(pq))
                {
                    kqThem = false;
                    break;
                }
            }

            // [BƯỚC 3]: Cập nhật Cache sau khi lưu thành công
            if (kqThem)
            {
                // A. Cập nhật ds (Chi tiết)
                // Bây giờ ds.Clear() an toàn vì ta đã có listSaoChep
                ds.Clear();
                foreach (var item in listSaoChep)
                {
                    ds.Add(item);
                }

                // B. Cập nhật dsHienThi (Danh sách tổng quát - nếu có dùng)
                if (dsHienThi != null)
                {
                    foreach (var itemMoi in listSaoChep)
                    {
                        var itemCu = dsHienThi.FirstOrDefault(x => x.MaVaiTro == itemMoi.MaVaiTro && x.MaQuyen == itemMoi.MaQuyen);
                        if (itemCu != null)
                        {
                            // Update đè lên item cũ để Grid tự refresh
                            int index = dsHienThi.IndexOf(itemCu);
                            dsHienThi[index] = itemMoi;
                        }
                        else
                        {
                            dsHienThi.Add(itemMoi);
                        }
                    }
                }
            }

            return kqThem;
        }

        public BindingList<phanquyenDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<phanquyenDTO> dskq = new BindingList<phanquyenDTO>();
            if(dsHienThi == null || dsHienThi.Count == 0)
            {
                LayDanhSach();
            }
            BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();
            BindingList<quyenDTO> dsQuyen = new quyenBUS().LayDanhSach();
            foreach (phanquyenDTO ct in dsHienThi)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaVaiTro.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {

                            vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == ct.MaVaiTro);
                            string tenVT = vt?.TenVaiTro ?? "";
                            if(tenVT.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            if(ct.MaQuyen.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            quyenDTO q = dsQuyen.FirstOrDefault(x => x.MaQuyen == ct.MaQuyen);
                            string tenQuyen = q?.TenQuyen ?? "";
                            if(tenQuyen.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            string can_create = ct.CAN_CREATE == 1 ? "Có" : "Không";
                            if(can_create.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 5:
                        {
                            string can_read = ct.CAN_READ == 1 ? "Có" : "Không";
                            if (can_read.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 6:
                        {
                            string can_update = ct.CAN_UPDATE == 1 ? "Có" : "Không";
                            if (can_update.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 7:
                        {
                            string can_delete = ct.CAN_DELETE == 1 ? "Có" : "Không";
                            if (can_delete.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<phanquyenDTO> timKiemNangCao(string tenVaiTro,string tenQuyen, int create, int read, int update, int delete)
        {
            BindingList<phanquyenDTO> dskq = new BindingList<phanquyenDTO>();
            BindingList<vaitroDTO> dsVaiTro = new vaitroBUS().LayDanhSach();
            BindingList<quyenDTO> dsQuyen = new quyenBUS().LayDanhSach();

            if(dsHienThi == null || dsHienThi.Count == 0)
            {
                LayDanhSach();
            }

            foreach(phanquyenDTO ct in dsHienThi)
            {
                bool dk = true;
                vaitroDTO vaitro = dsVaiTro.FirstOrDefault(x => x.MaVaiTro == ct.MaVaiTro);
                string tenVTtim = vaitro?.TenVaiTro ?? "";
                quyenDTO quyen = dsQuyen.FirstOrDefault(x => x.MaQuyen == ct.MaQuyen);
                string tenQuyentim = quyen?.TenQuyen ?? "";

                if(!string.IsNullOrEmpty(tenVaiTro) && tenVTtim.IndexOf(tenVaiTro, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }

                if(!string.IsNullOrEmpty(tenQuyen) && tenQuyentim.IndexOf(tenQuyen, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }

                if(create != -1 && ct.CAN_CREATE != create)
                {
                    dk = false;
                }

                if (read != -1 && ct.CAN_READ != read)
                {
                    dk = false;
                }

                if (update != -1 && ct.CAN_UPDATE != update)
                {
                    dk = false;
                }

                if (delete != -1 && ct.CAN_DELETE != delete)
                {
                    dk = false;
                }

                if(dk)
                {
                    dskq.Add(ct);
                }
            }
            return dskq;
        }
        // Hàm so sánh để tránh update thừa
        private bool LaPhanQuyenGiongNhau(phanquyenDTO cu, phanquyenDTO moi)
        {
            return cu.CAN_READ == moi.CAN_READ &&
                   cu.CAN_CREATE == moi.CAN_CREATE &&
                   cu.CAN_UPDATE == moi.CAN_UPDATE &&
                   cu.CAN_DELETE == moi.CAN_DELETE;
        }

        // Hàm kiểm tra giá trị 0/1 (BẠN ĐANG THIẾU HÀM NÀY)
        private bool LaGiaTriHopLe(int val)
        {
            return val == 0 || val == 1;
        }

        // 🟢 Logic nhập Excel thông minh
        public string NhapExcelThongMinh(BindingList<phanquyenDTO> dsExcel)
        {
            var dsDB = data.LayDanhSach(); // Lấy dữ liệu hiện tại từ DB
            vaitroDAO vaitroData = new vaitroDAO();
        quyenDAO quyenData = new quyenDAO();
        // ⚠️ LƯU Ý: Đảm bảo bên vaitroDAO và quyenDAO có hàm LayDanhSach()
        // Nếu tên hàm bên đó khác (vd: LayDanhSachVaiTro) thì bạn sửa lại cho khớp nhé
        var dsMaVaiTroDB = vaitroData.LayDanhSachVaiTro().Select(x => x.MaVaiTro).ToHashSet();
            var dsMaQuyenDB = quyenData.LayDanhSachQuyen().Select(x => x.MaQuyen).ToHashSet();

            BindingList<string> danhSachLoi = new BindingList<string>();
            HashSet<string> khoaChinhExcel = new HashSet<string>();

            // 1. Check lỗi dữ liệu
            foreach (var pq in dsExcel)
            {
                if (!dsMaVaiTroDB.Contains(pq.MaVaiTro))
                {
                    danhSachLoi.Add($"Dòng Excel: Mã vai trò {pq.MaVaiTro} không tồn tại.");
                }

                if (!dsMaQuyenDB.Contains(pq.MaQuyen))
                {
                    danhSachLoi.Add($"Dòng Excel: Mã quyền {pq.MaQuyen} không tồn tại.");
                }

                string key = $"{pq.MaVaiTro}-{pq.MaQuyen}";
                if (!khoaChinhExcel.Add(key))
                {
                    danhSachLoi.Add($"Cặp (Vai trò: {pq.MaVaiTro}, Quyền: {pq.MaQuyen}) bị lặp lại.");
                }

                if (!LaGiaTriHopLe(pq.CAN_READ) || !LaGiaTriHopLe(pq.CAN_CREATE) ||
                    !LaGiaTriHopLe(pq.CAN_UPDATE) || !LaGiaTriHopLe(pq.CAN_DELETE))
                {
                    danhSachLoi.Add($"Dòng (VT: {pq.MaVaiTro}, Q: {pq.MaQuyen}): Giá trị chỉ được là 0 hoặc 1.");
                }
            }

            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);
            }

            // 2. Thực hiện Thêm / Sửa / Bỏ qua
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var pqMoi in dsExcel)
            {
                var pqCu = dsDB.FirstOrDefault(x => x.MaVaiTro == pqMoi.MaVaiTro && x.MaQuyen == pqMoi.MaQuyen);

                if (pqCu == null)
                {
                    data.ThemPhanQuyen(pqMoi);
                    soThem++;
                }
                else if (!LaPhanQuyenGiongNhau(pqCu, pqMoi))
                {
                    // Gọi hàm Update đã viết bên DAO
                    data.CapNhatQuyen(pqMoi.MaVaiTro, pqMoi.MaQuyen, pqMoi.CAN_READ, pqMoi.CAN_CREATE, pqMoi.CAN_UPDATE, pqMoi.CAN_DELETE);
                    soCapNhat++;
                }
                else
                {
                    soBoQua++;
                }
            }

            LayDanhSach(); // Refresh lại list hiển thị
            return $"Hoàn tất!\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

        public BindingList<phanquyenDTO> LayQuyenCuaNhanVien(int maVaiTro)
        {
            return data.LayChiTietQuyenTheoVaiTro(maVaiTro);
        }
    }
}