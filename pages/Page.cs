using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Tab : Panel {
            public Tab() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Width = Dimensions.Width;
                Height = Dimensions.Height;
                BackColor = Color.FromArgb(0,0,0,0);
            }
        }
    }
}