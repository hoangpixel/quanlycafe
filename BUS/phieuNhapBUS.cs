using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;

namespace BUS
{
    public class phieuNhapBUS
    {
        private phieuNhapDAO pnDAO = new phieuNhapDAO();
        private ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();
        public static BindingList<phieuNhapDTO> ds;

        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            if (ds == null)
            {
                ds = pnDAO.LayDanhSach();
            }
            return ds;
        }

        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (header.MaNCC <= 0) throw new Exception("Vui lòng chọn Nhà cung cấp.");
            if (details == null || details.Count == 0) throw new Exception("Danh sách hàng nhập đang trống.");

            int newPhieuID = pnDAO.ThemPhieuNhap(header, details);
            if (newPhieuID > 0)
            {
                header.MaPN = newPhieuID;
                header.TongTien = details.Sum(x => x.ThanhTien);
                header.TrangThai = 0;
                var ds = LayDanhSach();
                ds.Insert(0, header);

            }

            return newPhieuID;
        }

        public bool DuyetPhieuNhap(int maPN)
        {
            bool result = pnDAO.DuyetPhieuNhap(maPN);
            if (result)
            {
                var phieu = LayDanhSach().FirstOrDefault(x => x.MaPN == maPN);
                if (phieu != null)
                {
                    phieu.TrangThai = 1;
                    ds.ResetItem(ds.IndexOf(phieu));
                }
            }
            return result;
        }

        public bool CapNhatThongTinPhieu(int maPN, int maNCC, int maNhanVien)
        {
            bool result = pnDAO.CapNhatThongTin(maPN, maNCC, maNhanVien);

            if (result)
            {
                var phieuCanSua = LayDanhSach().FirstOrDefault(x => x.MaPN == maPN);
                if (phieuCanSua != null)
                {
                    phieuCanSua.MaNCC = maNCC;
                    phieuCanSua.MaNhanVien = maNhanVien;

                    ds.ResetItem(ds.IndexOf(phieuCanSua));
                }
            }
            return result;
        }

        public bool ThemChiTietVaoPhieuCu(int mapn, List<ctPhieuNhapDTO> details)
        {
            if (mapn <= 0) throw new Exception("Mã phiếu nhập không hợp lệ.");
            if (details == null || details.Count == 0) throw new Exception("Chưa chọn nguyên liệu để thêm.");

            var phieuHeader = LayDanhSach().FirstOrDefault(x => x.MaPN == mapn);
            if (phieuHeader != null && phieuHeader.TrangThai == 1)
            {
                throw new Exception("Phiếu đã duyệt, không thể thêm chi tiết!");
            }

            bool result = pnDAO.ThemChiTietVaoPhieuCu(mapn, details);

            if (result)
            {
                if (phieuHeader != null)
                {
                    decimal tienMoiThem = details.Sum(x => x.ThanhTien);
                    phieuHeader.TongTien += tienMoiThem;
                    phieuHeader.ThoiGian = DateTime.Now;

                    ds.ResetItem(ds.IndexOf(phieuHeader));
                }
            }

            return result;
        }

        public bool CapNhatPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (header.MaPN <= 0) throw new Exception("Mã phiếu không hợp lệ");
            if (details == null || details.Count == 0) throw new Exception("Danh sách chi tiết trống");
            var phieuCu = LayDanhSach().FirstOrDefault(x => x.MaPN == header.MaPN);
            if (phieuCu != null && phieuCu.TrangThai == 1)
            {
                throw new Exception("Phiếu này đã nhập kho, không thể chỉnh sửa!");
            }
            header.TongTien = details.Sum(x => x.ThanhTien);
            bool result = pnDAO.CapNhatPhieuNhap(header, details);
            if (result && phieuCu != null)
            {
                phieuCu.MaNCC = header.MaNCC;
                phieuCu.MaNhanVien = header.MaNhanVien;
                phieuCu.TongTien = header.TongTien;
                phieuCu.ThoiGian = header.ThoiGian;

                ds.ResetItem(ds.IndexOf(phieuCu));
            }

            return result;
        }

        public List<ctPhieuNhapDTO> LayChiTiet(int mapn)
        {
            return ctDAO.LayDanhSachChiTietTheoMaPN(mapn);
        }
        public bool kiemTraChuoiRong(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                return true;
            }
            return false;
        }

        public bool XoaPhieuNhap(int maPN)
        {
            bool kq = pnDAO.XoaPhieuNhap(maPN);
            if (kq)
            {
                var tontai = ds.FirstOrDefault(x => x.MaPN == maPN);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        public void NhapExcelGop(List<DTO.PhieuNhapExcelRow> rawData)
        {
            if (rawData == null || rawData.Count == 0) return;

            var groups = rawData.GroupBy(x => x.MaPN_Excel);

            List<string> errors = new List<string>();
            int countSuccess = 0;

            var listNCC = new nhaCungCapBUS().LayDanhSach().ToList();
            var listNV = new nhanVienBUS().LayDanhSach().ToList();
            var listNL_Full = new nguyenLieuBUS().LayDanhSach().ToList();
            var listDonVi = new donViBUS().LayDanhSach().ToList();

            foreach (var group in groups)
            {
                var firstRow = group.First();
                int maPN_Excel = firstRow.MaPN_Excel;

                if (!listNCC.Any(x => x.MaNCC == firstRow.MaNCC))
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Mã NCC {firstRow.MaNCC} không tồn tại.");
                    continue;
                }
                if (!listNV.Any(x => x.MaNhanVien == firstRow.MaNV))
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Mã NV {firstRow.MaNV} không tồn tại.");
                    continue;
                }

                phieuNhapDTO header = new phieuNhapDTO
                {
                    MaPN = 0,
                    MaNCC = firstRow.MaNCC,
                    MaNhanVien = firstRow.MaNV,
                    ThoiGian = firstRow.ThoiGian,
                    TrangThai = 0,
                    TrangThaiXoa = 1,
                    TongTien = 0
                };

                List<ctPhieuNhapDTO> details = new List<ctPhieuNhapDTO>();
                bool hasDetailError = false;

                foreach (var row in group)
                {
                    if (row.MaNguyenLieu <= 0) continue;

                    var nlInfo = listNL_Full.FirstOrDefault(n => n.MaNguyenLieu == row.MaNguyenLieu);
                    if (nlInfo == null)
                    {
                        errors.Add($"Mã tạm {maPN_Excel}: Nguyên liệu ID {row.MaNguyenLieu} không tồn tại.");
                        hasDetailError = true; break;
                    }

                    int maDonViDB = nlInfo.MaDonViCoSo;
                    decimal heSo = 1;

                    if (!string.IsNullOrEmpty(row.TenDonVi))
                    {
                        var dvObj = listDonVi.FirstOrDefault(d => d.TenDonVi.Equals(row.TenDonVi.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (dvObj != null)
                        {
                            maDonViDB = dvObj.MaDonVi;
                            heSo = 1;
                        }
                    }

                    details.Add(new ctPhieuNhapDTO
                    {
                        MaNguyenLieu = row.MaNguyenLieu,
                        MaDonVi = maDonViDB,
                        SoLuong = row.SoLuong,
                        HeSo = heSo,
                        SoLuongCoSo = row.SoLuong * heSo,
                        DonGia = row.DonGia,
                        ThanhTien = row.SoLuong * row.DonGia
                    });
                }

                if (hasDetailError) continue;
                if (details.Count == 0)
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Không có chi tiết sản phẩm.");
                    continue;
                }

                try
                {
                    if (ThemPhieuNhap(header, details) > 0)
                    {
                        countSuccess++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Lỗi Insert - {ex.Message}");
                }
            }

            string msg = $"Đã nhập thành công: {countSuccess} phiếu.";
            if (errors.Count > 0)
            {
                msg += "\n\n--- CÁC LỖI BỎ QUA ---\n" + string.Join("\n", errors);
            }
            System.Windows.Forms.MessageBox.Show(msg, "Kết quả nhập Excel");
        }

        public BindingList<phieuNhapDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<phieuNhapDTO> dskq = new BindingList<phieuNhapDTO>();
            BindingList<nhaCungCapDTO> dsNCC = new nhaCungCapBUS().LayDanhSach();
            BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();

            foreach(phieuNhapDTO pn in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(pn.MaPN.ToString().Contains(tim))
                            {
                                dskq.Add(pn);
                            }
                            break;
                        }
                    case 1:
                        {
                            nhaCungCapDTO ncc = dsNCC.FirstOrDefault(x => x.MaNCC == pn.MaNCC);
                            string tenNCC = ncc?.TenNCC ?? "";
                            if(tenNCC.ToLower().IndexOf(tim,StringComparison.OrdinalIgnoreCase) > 0)
                            {
                                dskq.Add(pn);
                            }
                            break;
                        }
                    case 2:
                        {
                            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);
                            string tenNV = nv?.HoTen ?? "";
                            if(tenNV.ToLower().IndexOf(tim,StringComparison.OrdinalIgnoreCase) > 0)
                            {
                                dskq.Add(pn);
                            }
                            break;
                        }
                    case 3:
                        {
                            if(tim.ToLower().Equals("chưa"))
                            {
                                if(pn.TrangThai == 0)
                                {
                                    dskq.Add(pn);
                                }
                            }else if(tim.ToLower().Equals("rồi"))
                            {
                                if(pn.TrangThai == 1)
                                {
                                    dskq.Add(pn);
                                }
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<phieuNhapDTO> timKiemNangCao(int trangThaiPN,string tenNV,string tenNCC,DateTime timeBD,DateTime timeKT)
        {
            BindingList<phieuNhapDTO> dsTim = new BindingList<phieuNhapDTO>();
            BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
            BindingList<nhaCungCapDTO> dsNCC = new nhaCungCapBUS().LayDanhSach();

            foreach(phieuNhapDTO pn in ds)
            {
                nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);
                nhaCungCapDTO ncc = dsNCC.FirstOrDefault(x => x.MaNCC == pn.MaNCC);
                string tenNVtim = nv?.HoTen.ToLower() ?? "";
                string tenNCCtim = ncc?.TenNCC.ToLower() ?? "";

                bool kq = true;
                if(trangThaiPN != -1 && pn.TrangThai != trangThaiPN)
                {
                    kq = false;
                }
                if(!string.IsNullOrEmpty(tenNV) && tenNVtim.IndexOf(tenNV, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    kq = false;
                }
                if (!string.IsNullOrEmpty(tenNCC) && tenNCCtim.IndexOf(tenNCC, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    kq = false;
                }
                if (pn.ThoiGian.Date < timeBD.Date || pn.ThoiGian.Date > timeKT.Date)
                {
                    kq = false;
                }
                if (kq == true)
                {
                    dsTim.Add(pn);
                }
            }
            return dsTim;
        }
    }
    }