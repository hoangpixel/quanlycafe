using DTO;
using GUI.GUI_CRUD;
using GUI.GUI_UC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // =================================================================
            // 👇 BƯỚC 1: TẠO DỮ LIỆU GIẢ LẬP (MOCK DATA) ĐỂ KHÔNG BỊ LỖI NULL
            // =================================================================

            // Giả lập đang đăng nhập bằng Admin (Mã vai trò = 1)
            Session.TaiKhoanHienTai = new taikhoanDTO
            {
                TENDANGNHAP = "admin_test",
                MAVAITRO = 1, // 1 là Admin -> Sẽ hiện full nút
                MANHANVIEN = 1
            };

            // Giả lập thông tin nhân viên
            Session.NhanVienHienTai = new nhanVienDTO
            {
                MaNhanVien = 1,
                HoTen = "Developer Test"
            };

            // Nếu bạn đã làm phân quyền, cần khởi tạo list quyền rỗng hoặc full để tránh lỗi
            //Session.QuyenHienTai = new System.Collections.Generic.List<phanquyenDTO>();
            // (Vì MAVAITRO = 1 nên trong code Navbar mình đã cho qua luôn, không cần add quyền cụ thể vào list cũng được)

            // =================================================================
            // 👇 BƯỚC 2: CHẠY THẲNG VÀO MAIN GUI
            // =================================================================
            Application.Run(new mainGUI());

            // Application.Run(new dangNhapGUI()); // <-- Comment dòng cũ lại
        }
    }
}
