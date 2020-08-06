using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Tab : FlowLayoutPanel {
            public Tab() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                AutoSize = true;
                BackColor = Color.FromArgb(0,0,0,0);
                Padding = Style.BigPadding;
                Margin = Style.BigPadding;
            }
        }

        public partial class Home : Tab {
            public Home() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Home";

                var text = new Elements.Header();
                text.Text = "Welcome to GDSharp!";

                Controls.Add(text);
            }
        }
    }
}