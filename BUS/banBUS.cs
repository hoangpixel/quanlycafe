using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class banBUS
    {
        public static BindingList<banDTO> ds = new BindingList<banDTO>();
        private banDAO data = new banDAO();

        public BindingList<banDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSachBan();
            }
            return ds;
        }
        public bool DoiTrangThai(int maBan)
        {
            return data.DoiTrangThai(maBan);
        }
    }
}
