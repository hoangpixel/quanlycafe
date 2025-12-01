using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class detailPhanQuyen : Form
    {
        private phanquyenDTO pq;
        public detailPhanQuyen(phanquyenDTO pq)
        {
            InitializeComponent();
            this.pq = pq;
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void detailPhanQuyen_Load(object sender, EventArgs e)
        {
            BindingList<vaitroDTO> dsVaiTro = new vaitroBUS().LayDanhSach();
            BindingList<quyenDTO> dsQuyen = new quyenBUS().LayDanhSach();

            vaitroDTO vt = dsVaiTro.FirstOrDefault(x => x.MaVaiTro == pq.MaVaiTro);
            quyenDTO q = dsQuyen.FirstOrDefault(x => x.MaQuyen == pq.MaQuyen);

            txtVaiTro.Text = vt?.TenVaiTro ?? "Không xác định";
            txtQuyen.Text = q?.TenQuyen ?? "Không xác định";

            txtWrite.Text = pq.CAN_CREATE == 1 ? "Có" : "Không";
            txtRead.Text = pq.CAN_READ == 1 ? "Có" : "Không";
            txtUpdate.Text = pq.CAN_UPDATE == 1 ? "Có" : "Không";
            txtDelete.Text = pq.CAN_DELETE == 1 ? "Có" : "Không";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
