using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class phieuHuyBUS
    {
        private phieuHuyDAO data = new phieuHuyDAO();
        public static BindingList<phieuHuyDTO> ds = new BindingList<phieuHuyDTO>();

        public BindingList<phieuHuyDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.DocDanhSachPhieuHuy();
            }
            return ds;
        }

        public int layMa()
        {
            return data.LayMa();
        }

        public bool them(phieuHuyDTO ph, int maDonViNhap, decimal soLuongNhap)
        {
            bool kq = data.ThemPhieuHuyVaTruKho(ph, maDonViNhap, soLuongNhap);
            if (kq)
            {
                ph.NgayTao = DateTime.Now;
                ds.Insert(0, ph);
            }
            return kq;
        }
        public double LayHeSo(int maNL, int maDV)
        {
            return data.LayHeSoQuyDoiDonGian(maNL, maDV);
        }
    }
}