using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GDSharp {
    public class Style {
        public static int AppScale = 1;

        public static int TextSize = 10;
        public static int TextSizeHeader = 18;
        public static int PaddingSize = 4;
        public static int TransitionTime = 50;
        public static int CornerSize = 5;
        public static int TimerFPS = 10;
        public static int TabHeight = 30;
        public static int Margin = 5;

        public static Padding ButtonPadding = new Padding(PaddingSize * 4, PaddingSize, PaddingSize * 4, PaddingSize);
        public static Padding Padding = new Padding(PaddingSize, PaddingSize, PaddingSize, PaddingSize);
        public static Padding BigPadding = new Padding(PaddingSize * 2, PaddingSize * 2, PaddingSize * 2, PaddingSize * 2);

        public class Fonts {
            public static string Main = "OpenSans.ttf";
            public static string Head = "Montserrat.ttf";
        };

        private static readonly PrivateFontCollection _pfc = new PrivateFontCollection();

        public static void InitializeFonts() {
            _pfc.AddFontFile($"resources\\{Fonts.Main}");
            _pfc.AddFontFile($"resources\\{Fonts.Head}");
        }

        public static Font GetFont() {
            return new Font(_pfc.Families[1], Style.TextSize);
        }

        public static Font GetHeaderFont() {
            return new Font(_pfc.Families[0], Style.TextSizeHeader);
        }

        public class Colors : DefaultStyle {

        }

        public static Color Color(string hex) {
            return ColorTranslator.FromHtml(hex);
        }
    }
}