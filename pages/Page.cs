using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Tab : FlowLayoutPanel {
            public Tab() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Width = Dimensions.Width;
                Height = Dimensions.Height;
                BackColor = Color.FromArgb(0,0,0,0);
                Padding = Style.BigPadding;
                Margin = Style.BigPadding;
                FlowDirection = FlowDirection.TopDown;
            }
        }
    }
}