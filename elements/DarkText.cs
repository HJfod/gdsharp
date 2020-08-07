using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class TextDark : Label {
            public TextDark() {
                Font = Style.GetFont();
                AutoSize = true;
                ForeColor = Style.Color(Style.Colors.TextDark);
            }
        }
    }
}