using BUS;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml.Linq;
using Document = iTextSharp.text.Document;
using Paragraph = System.Windows.Documents.Paragraph;

namespace GUI.GUI_CRUD
{
    public partial class selectExcelHoaDon : Form
    {
        private int maHD_DangChon = -1;
        public selectExcelHoaDon()
        {
            InitializeComponent();
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
        }
    }
}
