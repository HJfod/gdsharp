using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class Header : Label {
            public Header() {
                Font = Style.GetHeaderFont();
                AutoSize = true;
                ForeColor = Style.Color(Style.Colors.Text);
            }
        }
        
        public partial class Text : Label {
            public Text() {
                Font = Style.GetFont();
                AutoSize = true;
                ForeColor = Style.Color(Style.Colors.Text);
            }
        }
    }
}