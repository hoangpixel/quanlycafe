using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DTO;

namespace DAO
{
    public class nhomDAO
    {
        public List<nhomDTO> LayDanhSach()
        {
            using (var db = new AppDbContext())
            {
                return db.Nhoms.Where(x => x.TrangThai == 1).ToList();
            }
        }
        public bool ThemNhom(nhomDTO ct)
        {
            using (var db = new AppDbContext())
            {
                nhomDTO moi = new nhomDTO();
                moi.TenNhom = ct.TenNhom;
                moi.TrangThai = 1;

                db.Nhoms.Add(moi);
                db.SaveChanges();
                return true;
            }
        }

        public int LayMa()
        {
            using (var db = new AppDbContext())
            {
                int maxMa = db.Nhoms.Select(x => x.MaNhom).DefaultIfEmpty(0).Max();
                return maxMa + 1;
            }
        }

        public bool SuaNhom(nhomDTO ct)
        {
            using (var db = new AppDbContext())
            {
                var item = db.Nhoms.FirstOrDefault(x => x.MaNhom == ct.MaNhom);

                if (item != null)
                {
                    item.TenNhom = ct.TenNhom;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool XoaNhom(int maNhom)
        {
            using (var db = new AppDbContext())
            {
                var item = db.Nhoms.FirstOrDefault(x => x.MaNhom == maNhom);
                if (item != null)
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