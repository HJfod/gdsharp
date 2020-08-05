using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;

namespace GDSharp {
    public class Style {
        public static int AppScale = 1;

        public static int TextSize = 10;
        public static int TextSizeHeader = 16;
        public static int Padding = 4;
        public static int TransitionTime = 5;
        public static int CornerSize = 5;

        public class Fonts {
            public static string Main = "OpenSans.ttf";
            public static string Head = "Montserrat.ttf";
        };

        public static Font GetFont() {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile($"resources\\{Fonts.Main}");
            return new Font(fontCollection.Families[0], Style.TextSize);
        }

        public static Font GetHeaderFont() {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile($"resources\\{Fonts.Head}");
            return new Font(fontCollection.Families[0], Style.TextSizeHeader);
        }

        public class Colors : DefaultStyle {

        }

        public static Color Color(string hex) {
            return ColorTranslator.FromHtml(hex);
        }
    }
}