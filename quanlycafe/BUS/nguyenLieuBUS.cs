using quanlycafe.DAO;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace quanlycafe.BUS
{
    internal class nguyenLieuBUS
    {
        public static List<nguyenLieuDTO> ds = new List<nguyenLieuDTO>();

        public List<nguyenLieuDTO> docDSNguyenLieu()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            return data.docDanhSachNguyenLieu();
        }

        public void napDSNguyenLieu()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            ds = data.docDanhSachNguyenLieu();
        }

        public bool themNguyenLieu(nguyenLieuDTO nl)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool kq = data.Them(nl);
            if (kq)
            {
                ds.Add(nl);
                Console.WriteLine($"BUS: Đã thêm nguyên liệu '{nl.TenNguyenLieu}' thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi thêm nguyên liệu!");
            }
            return kq;
        }

        public bool suaNguyenLieu(nguyenLieuDTO nl)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool result = data.Sua(nl);

            if (result)
            {
                var existing = ds.Find(x => x.MaNguyenLieu == nl.MaNguyenLieu);
                if (existing != null)
                {
                    existing.TenNguyenLieu = nl.TenNguyenLieu;
                    existing.DonViCoSo = nl.DonViCoSo;
                    existing.TonKho = nl.TonKho;
                    existing.TrangThai = nl.TrangThai;
                }
                Console.WriteLine("BUS: Sửa nguyên liệu thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi sửa nguyên liệu!");
            }

            return result;
        }

        public bool xoaNguyenLieu(int maNguyenLieu)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool result = data.Xoa(maNguyenLieu);

            if (result)
            {
                ds.RemoveAll(x => x.MaNguyenLieu == maNguyenLieu);
                Console.WriteLine("BUS: Đã ẩn nguyên liệu thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi ẩn nguyên liệu!");
            }

            return result;
        }


        // 🟢 Tìm theo mã
        public nguyenLieuDTO TimTheoMa(int ma)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            return data.TimTheoMa(ma);
        }

        // 🧹 Xóa toàn bộ dữ liệu (nếu cần làm mới khi nhập Excel)
        public void XoaTatCa()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();

            var ds = data.docDanhSachNguyenLieu();
            foreach (var nl in ds)
            {
                data.Xoa(nl.MaNguyenLieu);
            }
        }

        // 🧭 Kiểm tra 2 nguyên liệu có giống nhau không
        private bool LaNguyenLieuGiongNhau(nguyenLieuDTO a, nguyenLieuDTO b)
        {
            return a.TenNguyenLieu == b.TenNguyenLieu &&
                   a.DonViCoSo == b.DonViCoSo &&
                   a.TonKho == b.TonKho;
        }

        // 🧠 Nhập Excel thông minh: thêm / sửa / giữ nguyên
        public void NhapExcelThongMinh(List<nguyenLieuDTO> dsExcel)
        {
            int soThem = 0, soCapNhat = 0, soBoQua = 0, soLoi = 0, soTrungTen = 0;
            nguyenLieuDAO data = new nguyenLieuDAO();

            // 🔍 Lấy toàn bộ danh sách hiện tại 1 lần để so sánh
            var dsHienTai = data.docDanhSachNguyenLieu();

            foreach (var nlMoi in dsExcel)
            {
                try
                {
                    // ✅ Nếu trạng thái chưa có hoặc = 0 → tự động set lại = 1
                    if (nlMoi.TrangThai == 0)
                        nlMoi.TrangThai = 1;

                    // 🔎 Kiểm tra trùng tên nguyên liệu
                    bool tenTrung = dsHienTai.Any(n =>
                        string.Equals(n.TenNguyenLieu.Trim(), nlMoi.TenNguyenLieu.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (tenTrung)
                    {
                        // 🚫 Trùng tên → bỏ qua và ghi log
                        Console.WriteLine($"⚠️ Nguyên liệu '{nlMoi.TenNguyenLieu}' đã tồn tại → bỏ qua!");
                        soTrungTen++;
                        continue;
                    }

                    // 🔍 Kiểm tra theo mã nguyên liệu
                    var nlCu = data.TimTheoMa(nlMoi.MaNguyenLieu);

                    if (nlCu == null)
                    {
                        // 🆕 Chưa có → thêm mới
                        data.Them(nlMoi);
                        dsHienTai.Add(nlMoi); // cập nhật vào danh sách hiện tại để tránh trùng thêm lần sau
                        soThem++;
                    }
                    else if (!LaNguyenLieuGiongNhau(nlCu, nlMoi))
                    {
                        // 🔄 Có khác biệt → cập nhật
                        data.Sua(nlMoi);
                        soCapNhat++;
                    }
                    else
                    {
                        // ⚪ Giống hệt → bỏ qua
                        soBoQua++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi xử lý nguyên liệu Excel: " + ex.Message);
                    soLoi++;
                }
            }

            // 🧾 Tổng kết kết quả
            MessageBox.Show(
                $"✅ Nhập Excel hoàn tất!\n" +
                $"- {soThem} nguyên liệu mới được thêm.\n" +
                $"- {soCapNhat} nguyên liệu được cập nhật.\n" +
                $"- {soBoQua} nguyên liệu giữ nguyên.\n" +
                $"- {soTrungTen} nguyên liệu bị bỏ qua do trùng tên.\n" +
                $"- {soLoi} dòng bị lỗi.",
                "Kết quả nhập Excel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


    }
}
