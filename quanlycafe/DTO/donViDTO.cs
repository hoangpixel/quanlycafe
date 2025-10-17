namespace quanlycafe.DTO
{
    internal class donViDTO
    {
        public int MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public int TrangThai { get; set; } = 1;

        public donViDTO() { }

        public donViDTO(int maDonVi, string tenDonVi, int trangThai)
        {
            MaDonVi = maDonVi;
            TenDonVi = tenDonVi;
            TrangThai = trangThai;
        }
    }
}
