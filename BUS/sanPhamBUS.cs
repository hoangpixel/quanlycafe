using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class sanPhamBUS
    {
        public static List<sanPhamDTO> ds = new List<sanPhamDTO>();

        public void docDSSanPham()
        {
            sanPhamDAO data = new sanPhamDAO();
            ds = data.DocDanhSachSanPham();
        }

        public List<sanPhamDTO> layDanhSachSanPham()
        {
            sanPhamDAO data = new sanPhamDAO();
            return ds = data.DocDanhSachSanPham();
        }

        public bool them(sanPhamDTO ct)
        {
            sanPhamDAO data = new sanPhamDAO();
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }
        public void xoaTatCaSanPham()
        {
            sanPhamDAO dao = new sanPhamDAO();
            dao.xoaTatCa();
        }



        public bool Sua(sanPhamDTO sp)
        {
            sanPhamDAO data = new sanPhamDAO();
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
            sanPhamDAO data = new sanPhamDAO();
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
            sanPhamDAO data = new sanPhamDAO();
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
            sanPhamDAO spDAO = new sanPhamDAO();
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
                //MessageBox.Show(
                //    $"Dữ liệu Excel không hợp lệ, không thể nhập!\n\nLỗi phát hiện:\n• {loiHienThi}",
                //    "Lỗi dữ liệu Excel",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Error
                //);
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

            //MessageBox.Show(
            //    $"Nhập Excel thành công!\n" +
            //    $"- {soThem} sản phẩm mới được thêm.\n" +
            //    $"- {soCapNhat} sản phẩm được cập nhật.\n" +
            //    $"- {soBoQua} sản phẩm giữ nguyên.",
            //    "Kết quả nhập Excel",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information
            //);
        }


        public List<sanPhamDTO> timKiemCoBan(string tim, int index)
        {
            List<sanPhamDTO> kq = new List<sanPhamDTO>();

            if (ds == null)
                docDSSanPham();

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            List<loaiDTO> dsLoai = busLoai.layDanhSachLoai();

            nhomBUS busNhom = new nhomBUS();
            List<nhomDTO> dsNhom = busNhom.layDanhSach();

            foreach (sanPhamDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaSP.ToString().Contains(tim))
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.TenSP.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }                            
                            break;
                        }
                    case 2:
                        {
                            var loai = dsLoai.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
                            string tenLoai = loai != null ? loai.TenLoai : "";
                            if (tenLoai.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == ct.MaLoai);
                            string tenNhom = dsNhom.FirstOrDefault(n => n.MaNhom == (loai != null ? loai.MaNhom : -1))?.TenNhom ?? "";
                            if(tenNhom.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (float.TryParse(tim, out float giaMin))
                            {
                                if (ct.Gia >= giaMin)
                                {
                                    kq.Add(ct);
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            if (float.TryParse(tim, out float giaMax))
                            {
                                if (ct.Gia <= giaMax)
                                {
                                    kq.Add(ct);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            return kq;
        }

        public List<sanPhamDTO> timKiemNangCaoSP(int maLoai, int trangThaiCT, float giaMin, float giaMax, string tenSP)
        {
            var dskq = new List<sanPhamDTO>();
            if (ds == null) docDSSanPham();

            foreach (var ct in ds)
            {
                bool dk = true;

                if (maLoai != -1 && ct.MaLoai != maLoai) dk = false;
                if (trangThaiCT != -1 && ct.TrangThaiCT != trangThaiCT) dk = false;
                if (giaMin != -1 && ct.Gia < giaMin) dk = false;
                if (giaMax != -1 && ct.Gia > giaMax) dk = false;
                if (!string.IsNullOrEmpty(tenSP) &&
                    ct.TenSP.IndexOf(tenSP, StringComparison.OrdinalIgnoreCase) < 0) dk = false;

                if (dk) dskq.Add(ct);
            }

            return dskq;
        }

    }
}
