using quanlycafe.DAO;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace quanlycafe.BUS
{
    internal class congThucBUS
    {
        public static List<congThucDTO> ds = new List<congThucDTO>();

        public List<congThucDTO> docDSCongThucTheoSP(int maSP)
        {
            congThucDAO data = new congThucDAO();
            ds = data.docDanhSachCongThucTheoSP(maSP);
            return ds;
        }

        public List<sanPhamDTO> docDSSanPhamTheoNguyenLieu(int maNL)
        {
            congThucDAO data = new congThucDAO();
            return data.docDanhSachSanPhamTheoNguyenLieu(maNL);
        }


        public List<congThucDTO> docTatCaCongThuc()
        {
            congThucDAO data = new congThucDAO();
            return data.docTatCaCongThuc();
        }

        public bool themCongThuc(congThucDTO ct)
        {
            ct.TrangThai = 1;
            congThucDAO data = new congThucDAO();
            bool kq = data.ThemHoacCapNhat(ct);

            if (kq)
            {
                ds.Add(ct);
                Console.WriteLine($"BUS: Đã thêm công thức cho SP {ct.MaSanPham}, NL {ct.MaNguyenLieu}");

                // ✅ Cập nhật trạng thái CT cho sản phẩm
                SanPhamDAO spDAO = new SanPhamDAO();
                spDAO.CapNhatTrangThaiCT(ct.MaSanPham, 1);
            }

            return kq;
        }


        public bool suaCongThuc(congThucDTO ct)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Sua(ct);
            if (result)
            {
                var existing = ds.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham && x.MaNguyenLieu == ct.MaNguyenLieu);
                if (existing != null)
                {
                    existing.SoLuongCoSo = ct.SoLuongCoSo;
                    existing.TrangThai = ct.TrangThai;
                }
            }
            return result;
        }

        public bool xoaCongThuc(int maSP, int maNL)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Xoa(maSP, maNL);
            if (result)
            {
                ds.RemoveAll(x => x.MaSanPham == maSP && x.MaNguyenLieu == maNL);
            }
            return result;
        }


       
        // 🟢 Nhập Excel thông minh
        public void NhapExcelThongMinh(List<congThucDTO> dsExcel)
        {
            int soThem = 0, soCapNhat = 0, soBoQua = 0, soLoi = 0;
            congThucDAO data = new congThucDAO();
            foreach (var ctMoi in dsExcel)
            {
                try
                {
                    // Kiểm tra dữ liệu đầu vào
                    if (ctMoi.MaSanPham == 0 || ctMoi.MaNguyenLieu == 0)
                    {
                        soLoi++;
                        continue;
                    }

                    ctMoi.TrangThai = 1;

                    // Kiểm tra trùng mã (SP + NL)
                    var dsHienTai = data.docTatCaCongThuc();
                    var ctCu = dsHienTai.Find(x =>
                        x.MaSanPham == ctMoi.MaSanPham &&
                        x.MaNguyenLieu == ctMoi.MaNguyenLieu);

                    if (ctCu == null)
                    {
                        data.Them(ctMoi);
                        soThem++;
                    }
                    else if (Math.Abs(ctCu.SoLuongCoSo - ctMoi.SoLuongCoSo) > 0.0001f)
                    {
                        data.Sua(ctMoi);
                        soCapNhat++;
                    }
                    else
                        soBoQua++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi xử lý công thức Excel: " + ex.Message);
                    soLoi++;
                }
            }

            MessageBox.Show(
                $"✅ Nhập Excel hoàn tất!\n" +
                $"- {soThem} công thức mới được thêm.\n" +
                $"- {soCapNhat} công thức được cập nhật.\n" +
                $"- {soBoQua} công thức giữ nguyên.\n" +
                $"- {soLoi} dòng bị lỗi.",
                "Kết quả nhập Excel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
