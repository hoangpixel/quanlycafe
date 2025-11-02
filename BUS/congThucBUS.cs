using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;

namespace BUS
{
    public class congThucBUS
    {
        public static BindingList<congThucDTO> ds = new BindingList<congThucDTO>();

        public BindingList<congThucDTO> docDSCongThucTheoSP(int maSP)
        {
            congThucDAO data = new congThucDAO();
            ds = data.docDanhSachCongThucTheoSP(maSP);
            return ds;
        }

        public BindingList<congThucDTO> LayDanhSach()
        {
            congThucDAO data = new congThucDAO();
            if (ds == null || ds.Count == 0)
            {
                ds = data.layDanhSach();
            }
            return ds;
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

                // Cập nhật trạng thái CT cho sản phẩm
                sanPhamDAO spDAO = new sanPhamDAO();
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
                congThucDTO tontai = ds.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham && x.MaNguyenLieu == ct.MaNguyenLieu);
                if (tontai != null)
                {
                    tontai.SoLuongCoSo = ct.SoLuongCoSo;
                    tontai.MaDonViCoSo = ct.MaDonViCoSo;
                    tontai.TrangThai = ct.TrangThai;
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
                congThucDTO cth = ds.FirstOrDefault(x => x.MaSanPham == maSP && x.MaNguyenLieu == maNL);
                if (cth != null)
                {
                    ds.Remove(cth);
                }
            }
            return result;
        }



        // 🟢 Nhập Excel thông minh
        public void NhapExcelThongMinh(BindingList<congThucDTO> dsExcel)
        {
            int soThem = 0, soCapNhat = 0, soBoQua = 0, soLoi = 0;
            congThucDAO data = new congThucDAO();

            // 🆕 chỉ đọc 1 lần
            var dsHienTai = data.layDanhSach();

            foreach (var ctMoi in dsExcel)
            {
                try
                {
                    if (ctMoi.MaSanPham == 0 || ctMoi.MaNguyenLieu == 0)
                    {
                        soLoi++;
                        continue;
                    }

                    ctMoi.TrangThai = 1;

                    var ctCu = dsHienTai.FirstOrDefault(x =>
                        x.MaSanPham == ctMoi.MaSanPham &&
                        x.MaNguyenLieu == ctMoi.MaNguyenLieu);

                    if (ctCu == null)
                    {
                        data.Them(ctMoi);
                        soThem++;
                    }
                    else if (Math.Abs(ctCu.SoLuongCoSo - ctMoi.SoLuongCoSo) > 0.0001f ||
                             ctCu.MaDonViCoSo != ctMoi.MaDonViCoSo)
                    {
                        data.Sua(ctMoi);
                        soCapNhat++;
                    }
                    else soBoQua++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi xử lý công thức Excel: " + ex.Message);
                    soLoi++;
                }
            }

            //MessageBox.Show(
            //    $"✅ Nhập Excel hoàn tất!\n" +
            //    $"- {soThem} công thức mới được thêm.\n" +
            //    $"- {soCapNhat} công thức được cập nhật.\n" +
            //    $"- {soBoQua} công thức giữ nguyên.\n" +
            //    $"- {soLoi} dòng bị lỗi.",
            //    "Kết quả nhập Excel",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information
            //);
        }

    }
}