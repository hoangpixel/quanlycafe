using DTO;
using DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUS
{
    public class ppThanhToanBUS
    {
        public static BindingList<ppThanhToanDTO> ds = new BindingList<ppThanhToanDTO>();
        public ppThanhToanDAO data = new ppThanhToanDAO();
        public BindingList<ppThanhToanDTO> LayDanhSach()
        {
            if (ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }
    }
}
