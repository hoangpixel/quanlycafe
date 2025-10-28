using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace GUI.FONTS
{
    public static class FontManager
    {
        private static PrivateFontCollection privateFonts = new PrivateFontCollection();
        private static FontFamily regularFont;
        private static FontFamily boldFont;
        private static FontFamily lightFont;

        /// <summary>
        /// Gọi một lần khi ứng dụng khởi động để load font tùy chỉnh.
        /// </summary>
        public static void LoadFont()
        {
            try
            {
                string fontsFolder = System.IO.Path.Combine(Application.StartupPath, "FONTS");

                string regularPath = System.IO.Path.Combine(fontsFolder, "Saira-Regular.ttf");
                string boldPath = System.IO.Path.Combine(fontsFolder, "Saira-Bold.ttf");
                string lightPath = System.IO.Path.Combine(fontsFolder, "Saira-Light.ttf");

                // Load tất cả file font có sẵn
                privateFonts.AddFontFile(regularPath);
                privateFonts.AddFontFile(boldPath);
                privateFonts.AddFontFile(lightPath);

                // Dò tên chính xác (nhiều file Oswald có cùng Family name)
                foreach (var f in privateFonts.Families)
                {
                    string name = f.Name.ToLower();

                    if (name.Contains("bold"))
                        boldFont = f;
                    else if (name.Contains("light"))
                        lightFont = f;
                    else
                        regularFont = f;
                }

                // Nếu .NET không phân biệt (do cùng tên Oswald)
                if (regularFont == null && privateFonts.Families.Length > 0)
                    regularFont = privateFonts.Families[0];
                if (boldFont == null)
                    boldFont = regularFont;
                if (lightFont == null)
                    lightFont = regularFont;

                // Debug hiển thị kết quả
                Console.WriteLine("Font Regular: " + (regularFont?.Name ?? "null"));
                Console.WriteLine("Font Bold: " + (boldFont?.Name ?? "null"));
                Console.WriteLine("Font Light: " + (lightFont?.Name ?? "null"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể load font: " + ex.Message, "Lỗi Font", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static Font GetRegularFont(float size = 14, FontStyle style = FontStyle.Regular)
        {
            if (regularFont == null)
                throw new InvalidOperationException("Font Regular chưa được load. Hãy gọi FontManager.LoadFont() trước.");

            return new Font(regularFont, size, style);
        }

        public static Font GetBoldFont(float size = 14)
        {
            if (boldFont == null)
                throw new InvalidOperationException("Font Bold chưa được load. Hãy gọi FontManager.LoadFont() trước.");

            return new Font(boldFont, size, FontStyle.Bold);
        }

        public static Font GetLightFont(float size = 14)
        {
            if (lightFont == null)
                throw new InvalidOperationException("Font Light chưa được load. Hãy gọi FontManager.LoadFont() trước.");

            // Nếu .NET không phân biệt thì ép style Regular
            return new Font(lightFont, size, FontStyle.Regular);
        }

        public static void ApplyFontToAllControls(Control parent)
        {
            if (regularFont == null)
                throw new InvalidOperationException("Font chưa được load. Hãy gọi FontManager.LoadFont() trước.");

            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Font = new Font(regularFont, ctrl.Font.Size, ctrl.Font.Style);
                if (ctrl.HasChildren)
                    ApplyFontToAllControls(ctrl);
            }
        }
    }
}
