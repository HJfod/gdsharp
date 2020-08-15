using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class Header : Label {
            public Header(string _Text = null, Color? _C = null) {
                Font = Style.GetHeaderFont();
                AutoSize = true;
                ForeColor = _C is Color ? (Color) _C : Style.Color(Style.Colors.Text);
                if (_Text != null) Text = _Text;
            }
        }
        
        public partial class Text : Label {
            public Text(string _Text = null, Color? _C = null) {
                Font = Style.GetFont();
                AutoSize = true;
                ForeColor = _C is Color ? (Color) _C : Style.Color(Style.Colors.Text);
                if (_Text != null) Text = _Text;
            }
        }

        public partial class TextDark : Label {
            public TextDark(string _Text = null, Color? _C = null) {
                Font = Style.GetFont();
                AutoSize = true;
                ForeColor = _C is Color ? (Color) _C : Style.Color(Style.Colors.TextDark);
                if (_Text != null) Text = _Text;
            }
        }
    }
}