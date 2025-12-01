using DTO;
using BUS;
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
using System.Linq.Expressions;
using GUI.GUI_CRUD;

namespace GUI.GUI_UC
{
    public partial class phanQuyenGUI : UserControl
    {
        private phanquyenBUS bus = new phanquyenBUS();
        private BindingList<vaitroDTO> dsVaitro;
        private BindingList<quyenDTO> dsQuyen;
        public phanQuyenGUI()
        {
            InitializeComponent();
        }

        private void phanQuyenGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsVaitro = new vaitroBUS().LayDanhSach();
            dsQuyen = new quyenBUS().LayDanhSach();

            BindingList<phanquyenDTO> dsHienThi = new phanquyenBUS().LayDanhSach();
            loadDanhSachPhanQuyen(dsHienThi);
            loadFontChuVaSize();
        }

        private void loadDanhSachPhanQuyen(BindingList<phanquyenDTO> ds)
        {
            tbPhanQuyen.AutoGenerateColumns = false;
            tbPhanQuyen.Columns.Clear();

            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaVaiTro",
                HeaderText = "Vai trò"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaQuyen",
                HeaderText = "Quyền"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_READ",
                HeaderText = "READ"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_WRITE",
                HeaderText = "WRITE"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_UPDATE",
                HeaderText = "UPDATE"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_DELETE",
                HeaderText = "DELETE"
            });

            tbPhanQuyen.DataSource = ds;
            tbPhanQuyen.ReadOnly = true;
            btnChiTietPQ.Enabled = false;
        }

        private void tbPhanQuyen_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            phanquyenDTO pq = tbPhanQuyen.Rows[e.RowIndex].DataBoundItem as phanquyenDTO;

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "Vai trò")
            {
                vaitroDTO vt = dsVaitro.FirstOrDefault(x => x.MaVaiTro == pq.MaVaiTro);
                e.Value = vt?.TenVaiTro ?? "Không xác định";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "Quyền")
            {
                quyenDTO q = dsQuyen.FirstOrDefault(x => x.MaQuyen == pq.MaQuyen);
                e.Value = q?.TenQuyen ?? "Không xác định";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "READ")
            {
                e.Value = pq.CAN_READ == 1 ? "Có" : "Không";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "WRITE")
            {
                e.Value = pq.CAN_WRITE == 1 ? "Có" : "Không";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "UPDATE")
            {
                e.Value = pq.CAN_UPDATE == 1 ? "Có" : "Không";
            }


            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "DELETE")
            {
                e.Value = pq.CAN_DELETE == 1 ? "Có" : "Không";
            }
        }

        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbPhanQuyen.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbPhanQuyen.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbPhanQuyen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbPhanQuyen.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbPhanQuyen.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbPhanQuyen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbPhanQuyen.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbPhanQuyen.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbPhanQuyen.Refresh();
        }

        private void tbPhanQuyen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbPhanQuyen.ClearSelection();
        }

        private void btnCRUDPQ_Click(object sender, EventArgs e)
        {
            using (insertPhanQuyen form = new insertPhanQuyen())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnVaiTro_Click(object sender, EventArgs e)
        {
            using(vaiTroVaQuyenGUI form = new vaiTroVaQuyenGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
            dsQuyen = new quyenBUS().LayDanhSach();
            dsVaitro = new vaitroBUS().LayDanhSach();
        }
    }
}
