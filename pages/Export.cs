using System;
using System.Windows.Forms;
using System.Drawing;

namespace GDSharp {
    namespace Pages {
        public partial class Export : Tab {
            public Export() {
                InitializeComponent();
            }

            private void InitializeComponent() {
                Name = "Export";

                var button = new Elements.GButton();
                button.Text = "Export";
                button.Click += new EventHandler(OnClick);

                Controls.Add(button);
            }

            private void OnClick(object sender, EventArgs e) {
                Console.WriteLine("what do i look like, a blue ball?");
                new Throwaway().Focus();
            }
        }
    }
}