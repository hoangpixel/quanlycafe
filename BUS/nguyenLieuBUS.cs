using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class nguyenLieuBUS
    {
        public static BindingList<nguyenLieuDTO> ds = new BindingList<nguyenLieuDTO>();

        public BindingList<nguyenLieuDTO> LayDanhSach()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            ds = data.docDanhSachNguyenLieu();
            return ds;
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
                nguyenLieuDTO tontai = ds.FirstOrDefault(x => x.MaNguyenLieu == nl.MaNguyenLieu);
                if (tontai != null)
                {
                    tontai.TenNguyenLieu = nl.TenNguyenLieu;
                    tontai.MaDonViCoSo = nl.MaDonViCoSo;
                    tontai.TonKho = nl.TonKho;
                    tontai.TrangThai = nl.TrangThai;
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
                nguyenLieuDTO ct = ds.FirstOrDefault(x => x.MaNguyenLieu == maNguyenLieu);
                if(ct != null)
                {
                    ds.Remove(ct);
                }
            }
            return result;
        }


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
                   a.MaDonViCoSo == b.MaDonViCoSo &&
                   a.TonKho == b.TonKho;
        }

        // 🧠 Nhập Excel thông minh: thêm / sửa / giữ nguyên
        public void NhapExcelThongMinh(BindingList<nguyenLieuDTO> dsExcel)
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

            //MessageBox.Show(
            //    $"✅ Nhập Excel hoàn tất!\n" +
            //    $"- {soThem} nguyên liệu mới được thêm.\n" +
            //    $"- {soCapNhat} nguyên liệu được cập nhật.\n" +
            //    $"- {soBoQua} nguyên liệu giữ nguyên.\n" +
            //    $"- {soTrungTen} nguyên liệu bị bỏ qua do trùng tên.\n" +
            //    $"- {soLoi} dòng bị lỗi.",
            //    "Kết quả nhập Excel",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information
            //);
        }

        public BindingList<nguyenLieuDTO> timKiemCoBanNL(string tim,int index)
        {
            BindingList<nguyenLieuDTO> kq = new BindingList<nguyenLieuDTO>();
            if(ds == null)
            {
                LayDanhSach();
            }
            foreach (nguyenLieuDTO ct in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaNguyenLieu.ToString().Contains(tim))
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if(ct.TenNguyenLieu.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {

                            donViBUS bus = new donViBUS();
                            BindingList<donViDTO> dsdv = bus.LayDanhSach();
                            donViDTO donVi = dsdv.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                            string tenDV = donVi != null ? donVi.TenDonVi : "";
                            if (tenDV.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            float tonKhoMin = float.Parse(tim.ToString());
                            if(ct.TonKho >= tonKhoMin)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            float tonKhoMax = float.Parse(tim.ToString());
                            if(ct.TonKho <= tonKhoMax)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return kq;
        }
    }
}
