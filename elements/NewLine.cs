using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class NewLine : Label {
            public NewLine() {
                Height = 1;
                Width = Dimensions.Width;
            }
        }

        public partial class NewLineBig : Label {
            public NewLineBig() {
                Height = Style.TextSize * 3;
                Width = Dimensions.Width;
            }
        }
    }
}