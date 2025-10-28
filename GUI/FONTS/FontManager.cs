//using System;
//using SixLabors.Fonts;

//namespace GUI.FONTS
//{
//    public static class FontManager
//    {
//        private static readonly FontCollection _fontCollection = new FontCollection();
//        private static FontFamily _fontFamily;

//        /// <summary>
//        /// Gọi 1 lần khi chương trình khởi động để load font.
//        /// </summary>
//        public static void LoadFont()
//        {
//            try
//            {
//                string fontPath = "FONTS/JetBrainsMonoNL-Regular.ttf"; // đường dẫn tới file font
//                _fontFamily = _fontCollection.Add(fontPath);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Lỗi khi load font: " + ex.Message);
//            }
//        }

//        /// <summary>
//        /// Trả về font đã load với kích thước mong muốn.
//        /// </summary>
//        public static Font GetFont(float size = 14)
//        {
//            if (_fontFamily == null)
//                throw new InvalidOperationException("Chưa load font. Hãy gọi FontManager.LoadFont() trước.");

//            return _fontFamily.CreateFont(size);
//        }
//    }
//}
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GUI.FONTS
{
    public static class FontManager
    {
        private static PrivateFontCollection privateFonts = new PrivateFontCollection();

        // Gọi 1 lần khi app khởi động hoặc khi cần load font
        public static void LoadFont()
        {
            try
            {
                //string fontPath = @"FONTS\JetBrainsMonoNL-Regular.ttf";
                string fontPath = System.IO.Path.Combine(Application.StartupPath, "FONTS", "JetBrainsMonoNL-Regular.ttf");
                privateFonts.AddFontFile(fontPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể load font: " + ex.Message);
            }
        }

        // Lấy font với kích thước tuỳ ý
        public static Font GetFont(float size = 14, FontStyle style = FontStyle.Regular)
        {
            if (privateFonts.Families.Length == 0)
                throw new InvalidOperationException("Font chưa được load. Hãy gọi FontManager.LoadFont() trước.");

            return new Font(privateFonts.Families[0], size, style);
        }

        // Áp dụng font cho tất cả control trong form/usercontrol
        public static void ApplyFontToAllControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Font = new Font(privateFonts.Families[0], ctrl.Font.Size, ctrl.Font.Style);
                if (ctrl.HasChildren)
                    ApplyFontToAllControls(ctrl);
            }
        }
    }
}
