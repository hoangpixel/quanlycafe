using quanlycafe.DAO;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.BUS
{
    internal class sanPhamBUS
    {
        public static List<sanPhamDTO> ds = new List<sanPhamDTO>();

        public void docDSSanPham()
        {
            SanPhamDAO data = new SanPhamDAO();
            ds = data.DocDanhSachSanPham();
        }

        public List<sanPhamDTO> layDanhSachSanPham()
        {
            SanPhamDAO data = new SanPhamDAO();
            return ds = data.DocDanhSachSanPham();
        }

        public bool them(sanPhamDTO ct)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }
        public void xoaTatCaSanPham()
        {
            SanPhamDAO dao = new SanPhamDAO();
            dao.xoaTatCa();
        }



        public bool Sua(sanPhamDTO sp)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool result = data.Sua(sp);

            if (result)
            {
                var existing = ds.Find(x => x.MaSP == sp.MaSP);
                if (existing != null)
                {
                    existing.MaLoai = sp.MaLoai;
                    existing.TenSP = sp.TenSP;
                    existing.Hinh = sp.Hinh;
                    existing.TrangThai = sp.TrangThai;
                    existing.TrangThaiCT = sp.TrangThaiCT;
                    existing.Gia = sp.Gia;
                }
                Console.WriteLine("BUS: Sửa sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi sửa sản phẩm!");
            }

            return result;
        }

        public bool Xoa(int maSP)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool result = data.Xoa(maSP);

            if (result)
            {
                ds.RemoveAll(x => x.MaSP == maSP);
                Console.WriteLine("BUS: Xóa sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi xóa sản phẩm!");
            }

            return result;
        }

        public bool capNhatTrangThaiCT(int maSP, int trangThaiCT)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool result = data.CapNhatTrangThaiCT(maSP, trangThaiCT);

            if (result)
            {
                var sp = ds.FirstOrDefault(x => x.MaSP == maSP);
                if (sp != null)
                    sp.TrangThaiCT = trangThaiCT;

                Console.WriteLine($"BUS: Cập nhật trạng thái CT của sản phẩm {maSP} = {trangThaiCT}");
            }
            else
            {
                Console.WriteLine($"BUS: Lỗi cập nhật trạng thái CT cho sản phẩm {maSP}");
            }

            return result;
        }

        public bool LaSanPhamGiongNhau(sanPhamDTO a, sanPhamDTO b)
        {
            return a.MaLoai == b.MaLoai
                && a.TenSP.Trim() == b.TenSP.Trim()
                && Math.Abs(a.Gia - b.Gia) < 0.001f
                && (a.Hinh ?? "") == (b.Hinh ?? "");
        }
        public void NhapExcelThongMinh(List<sanPhamDTO> dsExcel)
        {
            SanPhamDAO spDAO = new SanPhamDAO();
            loaiSanPhamDAO loaiDAO = new loaiSanPhamDAO();

            // 🔹 Danh sách mã loại đang có trong DB
            var dsLoaiTonTai = loaiDAO.docDanhSachLoai().Select(l => l.MaLoai).ToList();

            // 🔹 Dùng để kiểm tra trùng mã SP trong file Excel
            HashSet<int> maSPDaGap = new HashSet<int>();
            List<string> danhSachLoi = new List<string>();

            // 🔍 Bước 1: Kiểm tra dữ liệu trước khi thêm
            foreach (var sp in dsExcel)
            {
                // ⚠️ Kiểm tra trùng mã trong Excel
                if (!maSPDaGap.Add(sp.MaSP))
                {
                    danhSachLoi.Add($"Mã SP {sp.MaSP} bị trùng trong file Excel (SP: {sp.TenSP}).");
                }

                // ⚠️ Kiểm tra mã loại hợp lệ
                if (!dsLoaiTonTai.Contains(sp.MaLoai))
                {
                    danhSachLoi.Add($"Mã loại {sp.MaLoai} của sản phẩm '{sp.TenSP}' không tồn tại trong DB.");
                }
            }

            // ❌ Nếu có lỗi → hiển thị cảnh báo và dừng
            if (danhSachLoi.Count > 0)
            {
                string loiHienThi = string.Join("\n• ", danhSachLoi);
                MessageBox.Show(
                    $"Dữ liệu Excel không hợp lệ, không thể nhập!\n\nLỗi phát hiện:\n• {loiHienThi}",
                    "Lỗi dữ liệu Excel",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return; // ⛔ Dừng, không thêm gì hết
            }

            // ✅ Bước 2: Nếu dữ liệu hợp lệ → tiến hành thêm/cập nhật
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var spMoi in dsExcel)
            {
                var spCu = spDAO.TimTheoMa(spMoi.MaSP);

                if (spCu == null)
                {
                    spDAO.Them(spMoi);
                    soThem++;
                }
                else if (!LaSanPhamGiongNhau(spCu, spMoi))
                {
                    spDAO.Sua(spMoi);
                    soCapNhat++;
                }
                else
                {
                    soBoQua++;
                }
            }

            MessageBox.Show(
                $"Nhập Excel thành công!\n" +
                $"- {soThem} sản phẩm mới được thêm.\n" +
                $"- {soCapNhat} sản phẩm được cập nhật.\n" +
                $"- {soBoQua} sản phẩm giữ nguyên.",
                "Kết quả nhập Excel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


    }
}
