using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MySql.Data.MySqlClient;
using DAO.CONFIG;
using System.ComponentModel;
using System.Data;

namespace DAO
{
    public class loaiSanPhamDAO
    {
        public List<loaiDTO> LayDanhSach()
        {
            using(var db = new AppDbContext())
            {
                return db.Loais.Where(x => x.TrangThai == 1).ToList();
            }
        }
        public int LayMa()
        {
            using(var db = new AppDbContext())
            {
                int maxMa = db.Loais.Select(x => x.MaLoai).DefaultIfEmpty(0).Max();
                return maxMa + 1;
            }
        }
        public bool them(loaiDTO ct)
        {
            using(var db = new AppDbContext())
            {
                loaiDTO newCt = new loaiDTO();
                newCt.TenLoai = ct.TenLoai;
                newCt.MaNhom = ct.MaNhom;
                newCt.TrangThai = 1;

                db.Loais.Add(newCt);
                db.SaveChanges();
                return true;
            }
        }

        public bool sua(loaiDTO ct)
        {
            using(var db = new AppDbContext())
            {
                loaiDTO item = db.Loais.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
                if(item != null)
                {
                    item.TenLoai = ct.TenLoai;
                    item.MaNhom = ct.MaNhom;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool xoa(int maXoa)
        {
            using(var db = new AppDbContext())
            {
                loaiDTO item = db.Loais.FirstOrDefault(x => x.MaLoai == maXoa);
                if(item != null)
                {
                    item.TrangThai = 0;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
