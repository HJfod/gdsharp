using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Elements {
        public partial class Container : FlowLayoutPanel {
            public Container() {
                Width = Dimensions.Width;
                Height = Dimensions.Height;
                BackColor = Color.FromArgb(0,0,0,0);
                Padding = Style.BigPadding;
                Margin = Style.BigPadding;
            }
        }
    }
}