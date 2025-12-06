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
    public class khuvucBUS
    {
        public static BindingList<khuVucDTO> ds = new BindingList<khuVucDTO>();
        private khuVucDAO data = new khuVucDAO();
        public BindingList<khuVucDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }
    }
}
