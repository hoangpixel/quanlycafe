using DAO;
using DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace BUS
{
    public class taikhoanBUS
    {
        private taikhoanDAO dao = new taikhoanDAO();

        public BindingList<taikhoanDTO> LayDanhSach()
        {
            return new BindingList<taikhoanDTO>(dao.LayDanhSach());
        }

        public bool Them(taikhoanDTO tk)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(tk.TENDANGNHAP))
                throw new System.Exception("Tên đăng nhập không được để trống!");

            if (string.IsNullOrWhiteSpace(tk.MATKHAU))
                throw new System.Exception("Mật khẩu không được để trống!");

            // Mã hóa mật khẩu
            tk.MATKHAU = MaHoaMD5(tk.MATKHAU);

            return dao.Them(tk);
        }

        public bool Sua(taikhoanDTO tk)
        {
            // Nếu có mật khẩu mới → Mã hóa
            if (!string.IsNullOrEmpty(tk.MATKHAU))
                tk.MATKHAU = MaHoaMD5(tk.MATKHAU);

            return dao.Sua(tk);
        }

        public bool Xoa(int mataikhoan)
        {
            return dao.Xoa(mataikhoan);
        }

        public taikhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            string matKhauMaHoa = MaHoaMD5(matKhau);
            return dao.DangNhap(tenDangNhap, matKhauMaHoa);
        }

        public List<KeyValuePair<int, string>> LayDanhSachVaiTro()
        {
            return dao.LayDanhSachVaiTro();
        }

        public List<KeyValuePair<int, string>> LayDanhSachNhanVienChuaCoTK()
        {
            return dao.LayDanhSachNhanVienChuaCoTK();
        }

        // Mã hóa MD5
        private string MaHoaMD5(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}