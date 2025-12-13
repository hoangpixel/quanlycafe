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
            if (ds == null || ds.Count == 0 || (ds.Count > 0 && ds[0].MaVaiTro != maVaiTro))
            {
                ds = data.LayChiTietQuyenTheoVaiTro(maVaiTro);
            }
            return ds;
        }

        public bool LuuPhanQuyen(int maVaiTro, BindingList<phanquyenDTO> dsQuyenMoi)
        {
            List<phanquyenDTO> listSaoChep = dsQuyenMoi.ToList();

            bool kqXoa = data.XoaToanBoQuyenCuaVaiTro(maVaiTro);
            if (!kqXoa) return false;

            bool kqThem = true;
            foreach (var pq in listSaoChep)
            {
                pq.MaVaiTro = maVaiTro;
                if (!data.ThemPhanQuyen(pq))
                {
                    kqThem = false;
                    break;
                }
            }

            if (kqThem)
            {
                ds.Clear();
                foreach (var item in listSaoChep)
                {
                    ds.Add(item);
                }

                if (dsHienThi != null)
                {
                    foreach (var itemMoi in listSaoChep)
                    {
                        var itemCu = dsHienThi.FirstOrDefault(x => x.MaVaiTro == itemMoi.MaVaiTro && x.MaQuyen == itemMoi.MaQuyen);
                        if (itemCu != null)
                        {
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
        private bool LaPhanQuyenGiongNhau(phanquyenDTO cu, phanquyenDTO moi)
        {
            return cu.CAN_READ == moi.CAN_READ &&
                   cu.CAN_CREATE == moi.CAN_CREATE &&
                   cu.CAN_UPDATE == moi.CAN_UPDATE &&
                   cu.CAN_DELETE == moi.CAN_DELETE;
        }

        private bool LaGiaTriHopLe(int val)
        {
            return val == 0 || val == 1;
        }
        public bool ThemPhanQuyenDonLe(phanquyenDTO pq)
        {
            return data.ThemPhanQuyen(pq);
        }
        public bool CapNhatPhanQuyenDonLe(phanquyenDTO pq)
        {
            return data.CapNhatQuyen(pq.MaVaiTro, pq.MaQuyen, pq.CAN_READ, pq.CAN_CREATE, pq.CAN_UPDATE, pq.CAN_DELETE);
        }
        public string NhapExcelThongMinh(BindingList<phanquyenDTO> dsExcel)
        {
            vaitroBUS busVaiTro = new vaitroBUS();
            quyenBUS busQuyen = new quyenBUS();

            // Lấy Hashset ID để tra cứu cho nhan
            var dsMaVaiTroDB = busVaiTro.LayDanhSach().Select(x => x.MaVaiTro).ToHashSet();
            var dsMaQuyenDB = busQuyen.LayDanhSach().Select(x => x.MaQuyen).ToHashSet();

            // Lấy danh sách phân quyền hiện tại từ DB để so sánh
            var dsDB = LayDanhSach();

            BindingList<string> danhSachLoi = new BindingList<string>();
            HashSet<string> khoaChinhExcel = new HashSet<string>();

            // 2. CHECK LỖI DỮ LIỆU
            foreach (var pq in dsExcel)
            {
                // Check tồn tại Mã Vai Trò
                if (!dsMaVaiTroDB.Contains(pq.MaVaiTro))
                    danhSachLoi.Add($"Dòng Excel: Mã vai trò {pq.MaVaiTro} không tồn tại.");

                // Check tồn tại Mã Quyền
                if (!dsMaQuyenDB.Contains(pq.MaQuyen))
                    danhSachLoi.Add($"Dòng Excel: Mã quyền {pq.MaQuyen} không tồn tại.");

                // Check trùng lặp cặp khóa chính (VaiTro + Quyen) trong file Excel
                string key = $"{pq.MaVaiTro}-{pq.MaQuyen}";
                if (!khoaChinhExcel.Add(key))
                    danhSachLoi.Add($"Cặp (Vai trò: {pq.MaVaiTro}, Quyền: {pq.MaQuyen}) bị lặp lại.");

                // Check giá trị hợp lệ (chỉ 0 hoặc 1)
                if (!LaGiaTriHopLe(pq.CAN_READ) || !LaGiaTriHopLe(pq.CAN_CREATE) ||
                    !LaGiaTriHopLe(pq.CAN_UPDATE) || !LaGiaTriHopLe(pq.CAN_DELETE))
                {
                    danhSachLoi.Add($"Dòng (VT: {pq.MaVaiTro}, Q: {pq.MaQuyen}): Giá trị phân quyền chỉ được là 0 hoặc 1.");
                }
            }

            if (danhSachLoi.Count > 0)
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);

            // 3. THỰC HIỆN NHẬP (Thêm / Sửa)
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var pqMoi in dsExcel)
            {
                // Tìm xem cặp quyền này đã có trong DB chưa
                var pqCu = dsDB.FirstOrDefault(x => x.MaVaiTro == pqMoi.MaVaiTro && x.MaQuyen == pqMoi.MaQuyen);

                if (pqCu == null)
                {
                    // === THÊM MỚI ===
                    if (ThemPhanQuyenDonLe(pqMoi))
                    {
                        // Thêm vào cache hiển thị
                        dsHienThi.Add(pqMoi);
                        soThem++;
                    }
                }
                else
                {
                    // === CẬP NHẬT ===
                    if (!LaPhanQuyenGiongNhau(pqCu, pqMoi))
                    {
                        if (CapNhatPhanQuyenDonLe(pqMoi))
                        {
                            // Cập nhật cache hiển thị
                            pqCu.CAN_READ = pqMoi.CAN_READ;
                            pqCu.CAN_CREATE = pqMoi.CAN_CREATE;
                            pqCu.CAN_UPDATE = pqMoi.CAN_UPDATE;
                            pqCu.CAN_DELETE = pqMoi.CAN_DELETE;

                            soCapNhat++;
                        }
                    }
                    else
                    {
                        soBoQua++;
                    }
                }
            }

            // Refresh lại danh sách tổng lần cuối cho chắc chắn
            LayDanhSach();

            return $"Hoàn tất!\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

        public BindingList<phanquyenDTO> LayQuyenCuaNhanVien(int maVaiTro)
        {
            return data.LayChiTietQuyenTheoVaiTro(maVaiTro);
        }
    }
}